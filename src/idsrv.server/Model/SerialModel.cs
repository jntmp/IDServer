namespace idsrv.server.Model
{
    public class SerialModel
    {
        public string Id { get; set; }
        public bool Active { get; set; }

        public SerialModel(string id, bool active)
        {
            Id = id;
            Active = active;
        }
    }
}
