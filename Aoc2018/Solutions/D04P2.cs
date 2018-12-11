using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2018.Solutions
{
    public class D04P2 : Solver<int>
    {
        protected override string Filename => @"Inputs\d04.input";
        public override int Day => 4;
        public override int Part => 2;

        // Some serious sub-optimal linq abuse going on here
        protected override int GetAnswer(string input)
        {
            var events = ParseInput(input);
            var orderedEvents = events.OrderBy(e => e.Month)
                                      .ThenBy(e => e.Day)
                                      .ThenBy(e => e.Hour)
                                      .ThenBy(e => e.Minute)
                                      .ToArray();

            var guardInfos = new Dictionary<int, GuardInfo>();
            var currentGuardId = 0;

            for (var i = 0; i < orderedEvents.Length - 1; i++)
            {
                var evnt = orderedEvents[i];

                switch (evnt.EventType)
                {
                    case EventType.BeginShift:

                        currentGuardId = evnt.GuardId.Value;

                        if (!guardInfos.ContainsKey(currentGuardId))
                            guardInfos[currentGuardId] = new GuardInfo { GuardId = currentGuardId };
                        break;

                    case EventType.WakeUp:
                        // Nothing to do, only care about FallAsleep -> Wakeup Transitions
                        // Guards appear to always wake up before next guard shift
                        break;

                    case EventType.FallAsleep:
                        var wakeUpEvent = orderedEvents[i + 1];
                        if (wakeUpEvent.EventType != EventType.WakeUp)
                            throw new Exception("Your assumption that a wake up event always follows fall asleep is wrong.");

                        for (var m = evnt.Minute; m < wakeUpEvent.Minute; m++)
                        {
                            guardInfos[currentGuardId].MinuteSleepCounter[m]++;
                            guardInfos[currentGuardId].TotalMinutesSlept++;
                        }

                        break;
                }
            }

            var mostSlept = 0;
            var answer = 0;

            foreach (var guard in guardInfos.Values)
            {
                if (guard.MostSleptMinuteValue > mostSlept)
                {
                    mostSlept = guard.MostSleptMinuteValue;
                    answer = guard.Answer;
                }
            }

            return answer;
        }

        private class GuardInfo
        {
            public int GuardId { get; set; }
            public int[] MinuteSleepCounter { get; } = new int[60]; // 00-59
            public int MostSleptMinute => Array.IndexOf(MinuteSleepCounter, MinuteSleepCounter.Max());
            public int MostSleptMinuteValue => MinuteSleepCounter.Max();
            public int TotalMinutesSlept = 0;
            public int Answer => MostSleptMinute * GuardId;
        }

        private GuardEvent[] ParseInput(string input)
        {
            var events = new List<GuardEvent>();

            var inputRegex = new Regex(@"\[1518-(\d+)-(\d+) (\d+):(\d+)\] (.+)");
            var guardRegex = new Regex(@"Guard #(\d+) begins shift");
            var matches = input.Split(Environment.NewLine).Select(i => inputRegex.Match(i));

            foreach (var match in matches)
            {
                var ge = new GuardEvent
                {
                    Month = int.Parse(match.Groups[1].ToString()),
                    Day = int.Parse(match.Groups[2].ToString()),
                    Hour = int.Parse(match.Groups[3].ToString()),
                    Minute = int.Parse(match.Groups[4].ToString())
                };

                var eventText = match.Groups[5].ToString();

                if (guardRegex.IsMatch(eventText))
                {
                    var guardMatch = guardRegex.Match(eventText);
                    ge.GuardId = int.Parse(guardMatch.Groups[1].ToString());
                    ge.EventType = EventType.BeginShift;
                }
                else if (eventText == "wakes up")
                {
                    ge.EventType = EventType.WakeUp;
                }
                else if (eventText == "falls asleep")
                {
                    ge.EventType = EventType.FallAsleep;
                }
                else
                {
                    throw new Exception("You didn't parse event input correctly.");
                }

                events.Add(ge);
            }

            return events.ToArray();
        }

        private class GuardEvent
        {
            public EventType EventType { get; set; }
            public int? GuardId { get; set; }
            public int Month { get; set; }
            public int Day { get; set; }
            public int Hour { get; set; }
            public int Minute { get; set; }

            public override string ToString()
            {
                return $"[{Month}-{Day} {Hour}:{Minute}] #{(GuardId.HasValue ? GuardId.ToString() : "???")} {EventType}";
            }
        }

        private enum EventType
        {
            BeginShift,
            FallAsleep,
            WakeUp
        }
    }
}
