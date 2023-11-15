namespace Hermes.Models { 

    public partial class ChamadoModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int User_Id { get; set; }
        public string Descricao { get; set; }

        public ChamadoModel()
        {
            if(Descricao == null)
            {
                Descricao = "";
            }
        }
    }

}