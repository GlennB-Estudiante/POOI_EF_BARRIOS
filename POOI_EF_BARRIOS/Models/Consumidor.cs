namespace POOI_EF.Models
{
    public class Consumidor
    {
        public string dnicont { get; set; }
        public string nomcont { get; set; }
        public string apecont { get; set; }
        public string emailcont { get; set; }
        public string idpais { get; set; }
    }

    public class EmpleadoPedidos
    {
        public int idempleado { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }
    }
}
