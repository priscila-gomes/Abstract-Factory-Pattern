using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Abstract_Factory_Pattern
{
    //-------------AbstractFactory->  Interface ->IAdonet
    //-------------ConcreteFactory->  Class->     DbSqlServer, DbAccess
    //-------------AbstractProduct->  Interface ->IDbConnection, IDbCommand, IDataReader
    //-------------Product->          Class->     SqlConnection, SqlCommand, SqlDataReader,
    //                                            OleDbConnection, OleDbCommand, OleDbDataReader
    //-------------Client->           Class     ->DbUser

    interface IAdonet
    {
        void TestDatabase(); 
    }
    class DbSqlServer : IAdonet
    {
        public void TestDatabase() 
        {
            IDbConnection conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SchoolDB;Integrated Security=SSPI");
            string queryString = "SELECT [Id],[FirstName],[LastName],[Class] FROM [SchoolDB].[dbo].[Teacher];";
            
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = queryString;
            conn.Open();
            
            IDataReader dtReader = cmd.ExecuteReader(); 

            while (dtReader.Read())
            {               
                IDataRecord record = (IDataRecord)dtReader;
                Console.WriteLine(String.Format("Id:{0}, Firstname:{1}, Lastname:{2}, Class:{3}", record[0], record[1], record[2], record[3]));
            }
            dtReader.Close();
            conn.Close();                
        }
    }

    class DbAccess : IAdonet
    {
        public void TestDatabase()
        {
            IDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source =E:\\IDE - Visual Studio\\_ASSIGNMENT\\Abstract Factory Pattern\\SchoolDB.mdb");
            string queryString = "SELECT [Id],[FirstName],[LastName],[Class] FROM [Teacher];";

            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = queryString;
            conn.Open();

            IDataReader dtReader = cmd.ExecuteReader();

            while (dtReader.Read())
            {
                IDataRecord record = (IDataRecord)dtReader;
                Console.WriteLine(String.Format("Id:{0}, Firstname:{1}, Lastname:{2}, Class:{3}", record[0], record[1], record[2], record[3]));
            }
            dtReader.Close();
            conn.Close();
        }
    }

    class DbUser
    {
        //IConnection dbConn;
        //ICommand dbCom;  
        //IDataReader dbDtReader;

        public DbUser(IAdonet factory)
        {
            factory.TestDatabase();
            //dbCom = factory.Command(); 
            //dbDtReader = factory.DataReader(); 
        }

        /*public string ConnectionDetails()
        {
            return "Database Connected";
            //return dbConn.dbConnection();  
        }*/

        /*public string CommandDetails()  
        {  
            return dbCom.dbCommand();  
        }  
        public string DtReaderDetails()  
        {  
            return dbDtReader.dbDataReader();  
        }*/
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("********* SQL SERVER Database **********");
            IAdonet sqlServerDb = new DbSqlServer();
            DbUser sqlServerUser = new DbUser(sqlServerDb);

            Console.WriteLine("\n********* ACCESS Database **********"); 
            IAdonet accessDb = new DbAccess();
            DbUser accessUser = new DbUser(accessDb);
            
            //Console.WriteLine(accessUser.ConnectionDetails());

            //Console.WriteLine(sqlServerUser.ConnectionDetails()); 


            /*Console.WriteLine(sqlServerUser.CommandDetails()); 
            Console.WriteLine(sqlServerUser.DtReaderDetails()); */
        }
    }
}
