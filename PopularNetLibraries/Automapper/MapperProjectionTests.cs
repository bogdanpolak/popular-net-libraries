using System;
using AutoMapper;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace PopularNetLibraries.Automapper
{
    public class MapperProjectionTests
    {
        private class EventRequest
        {
            public string StartDay;
            public string StartTime;
            public int DurationMin;
            public string Title;
        }
        private class Event
        {
            public DateTime Start;
            public TimeSpan Duration;
            public string Title;
        }
        [Fact]
        public void EventProjection()
        {
            var config = new MapperConfiguration(expression => 
                expression.CreateMap<EventRequest, Event>()
                    .ForMember(
                        dest => dest.Start, 
                        opt=> opt.MapFrom(
                            src=> DateTime.ParseExact(
                                $"{src.StartDay} {src.StartTime}",
                                "yyyy-MM-dd HH:mm",
                                System.Globalization.CultureInfo.InvariantCulture))
                        )
                    .ForMember(dest=>dest.Duration,
                        opt=>opt.MapFrom(
                            src=> TimeSpan.FromMinutes(src.DurationMin)))
                );
            var mapper = config.CreateMapper();

            var eventRequest = new EventRequest
            {
                StartDay = "2021-07-17", 
                StartTime = "18:30", 
                DurationMin = 90, 
                Title = "Meeting with friends"
            };
            var event1 = mapper.Map<Event>(eventRequest);

            event1.Should().BeEquivalentTo(new Event
            {
                Start = 17.July(2021).At(18,30), 
                Duration = new TimeSpan(1, 30, 0),
                Title = eventRequest.Title
            });
        }
    }
}