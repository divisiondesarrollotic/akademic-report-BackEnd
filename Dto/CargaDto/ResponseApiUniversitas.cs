namespace AkademicReport.Dto.CargaDto
{
    public class ResponseApiUniversitas
    {
        public class Env
        {
            public string defaultTimeZone { get; set; }

        }
        public class StatementPos
        {
            public int startLine { get; set; }
            public int endLine { get; set; }

        }
        public class Metadata
        {
            public string columnName { get; set; }
            public string jsonColumnName { get; set; }
            public string columnTypeName { get; set; }
            public string columnClassName { get; set; }
            public int precision { get; set; }
            public int scale { get; set; }
            public int isNullable { get; set; }

        }
        public class Items
        {
            public int vac_codnum { get; set; }
            public int ass_codnum { get; set; }
            public string id_grp_activ { get; set; }
            public string desid1 { get; set; }
            public int? cpcgrp { get; set; }
            public int numcre { get; set; }
            public string id_assignatura { get; set; }
            public string nomid1 { get; set; }
            public string secc { get; set; }
            public string any_anyaca { get; set; }
            public int dsm_codnum { get; set; }
            public string recinto { get; set; }
            public int codnum { get; set; }
            public int horini { get; set; }
            public int minini { get; set; }
            public int horfin { get; set; }
            public int minfin { get; set; }
            public double numhor { get; set; }
            public string aul_desc { get; set; }
            public string identificador { get; set; }
            public string descripcion { get; set; }

        }
        public class ResultSet
        {
            public IList<Metadata> metadata { get; set; }
            public IList<Items> items { get; set; }
            public bool hasMore { get; set; }
            public int limit { get; set; }
            public int offset { get; set; }
            public int count { get; set; }

        }
        public class ItemsOupSise
        {
            public int statementId { get; set; }
            public string statementType { get; set; }
            public StatementPos statementPos { get; set; }
            public string statementText { get; set; }
            public ResultSet resultSet { get; set; }
            public IList<Items> response { get; set; }
            public int result { get; set; }

        }
        public class Application
        {
            public Env env { get; set; }
            public IList<ItemsOupSise> items { get; set; }

        }
    }
}

