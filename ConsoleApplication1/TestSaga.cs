using System;
using NServiceBus;
using NServiceBus.Saga;

namespace ConsoleApplication1
{
    public class TestEvent : IEvent
    {
        public string AggregateId { get; set; }
    }
    public class EmptyData : ContainSagaData
    {
        public string EmptyId { get; set; }
    }
    public class TestSaga : Saga<EmptyData>, IAmStartedByMessages<TestEvent>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<EmptyData> mapper)
        {
            mapper.ConfigureMapping<TestEvent>(x => x.AggregateId).ToSaga(x => x.EmptyId);
        }
        public void Handle(TestEvent message)
        {
            throw new NotImplementedException();
        }
    }
}
