using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace LSM.Generic.Repository.Tests
{
    [TestClass]
    public class UnitTest1
    {

        public class Teste
        {
            public decimal Dec1 { get; set; }

            public decimal? Dec2 { get; set; }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var dt = new DataTable();
            dt.Columns.Add("Dec1");
            dt.Columns.Add("Dec2");

            var row = dt.NewRow();

            row["Dec1"] = 7;
            row["Dec2"] = "7,5";

            dt.Rows.Add(row);

            row = dt.NewRow();

            row["Dec1"] = 7;
            row["Dec2"] = null;

            dt.Rows.Add(row);

            var lista = DtMapper.DataTableToList<Teste>(dt);

            var a = "";


        }
    }
}
