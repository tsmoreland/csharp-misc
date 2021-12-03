namespace TSMoreland.ObjectTracker.Data.Abstractions.Entities
{
    public sealed class ObjectEntity 
    {

        public ObjectEntity(int id, string name, List<LogEntity> logs)
        {
            Id = id;
            Name = name;
            Logs = logs;
        }
        private ObjectEntity()
        {
        }

        public int Id { get; private set; }
        public string Name { get; set; } = string.Empty;
        public List<LogEntity> Logs { get; } = new();

    }
}
