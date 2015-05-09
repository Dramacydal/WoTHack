
namespace WoTHack
{
    class ProcessInfo
    {
        public ProcessInfo(int id, string name)
        {
            Id = id;
            Name = name;
            DisplayName = name + " - " + id;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
