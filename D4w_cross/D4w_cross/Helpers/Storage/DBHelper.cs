using D4w_cross.Models.API;
using D4w_cross.Models.API.Search;
using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.IO;

namespace D4w_cross.Helpers
{

    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }

    class DBHelper 
    {
        private const String DB_NAME = "localdb.db3";
        private SQLiteConnection database;
        private static volatile DBHelper instance;
        private static object syncRoot = new Object();

        private SQLiteConnection Connect()
        {
            //IFolder folder = FileSystem.Current.LocalStorage;
            //string path = PortablePath.Combine(folder.Path.ToString(), DB_NAME);

#if __IOS__
            string folder = Environment.GetFolderPath
              (Environment.SpecialFolder.Personal);
            folder = Path.Combine(folder, "..", "Library");
            var databasePath = Path.Combine(folder, DB_NAME);
#else
#if __ANDROID__
            string folder = Environment.GetFolderPath
              (Environment.SpecialFolder.Personal);
            var databasePath = Path.Combine(folder, DB_NAME);
#endif
#endif

            return new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
        }

        private DBHelper()
        {
            database = Connect();
            //database.DropTable<Models.API.Image>();
            database.CreateTable<Token>();
            database.CreateTable<Models.API.Image>();
            database.CreateTable<Booking>();
            database.CreateTable<CoworkingSearchOptions>();
        }

        public static DBHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DBHelper();
                    }
                }
                return instance;
            }
        }

        public void SetToken(Token token)
        {
            database.InsertOrReplace(token);
        }

        public Token GetToken()
        {
            return database.Table<Token>().FirstOrDefault();
        }

        public void DeleteToken()
        {
            database.DeleteAll<Token>();
        }

        public void SaveImage(Models.API.Image image)
        {
            database.InsertOrReplace(image);
        }

        public Models.API.Image FindImage(int id)
        {
            return database.Table<Models.API.Image>().FirstOrDefault(image => image.Id == id);
        }

        public void SaveCoworking(Coworking coworking)
        {
            database.InsertOrReplace(coworking);
        }

        public Coworking FindCoworking(int id)
        {
            return database.Table<Coworking>().FirstOrDefault(cwrk => cwrk.Id == id);
        }

        public Booking GetFutureBooking()
        {
            var books = database.Table<Booking>().FirstOrDefault();
            return database.Table<Booking>().Where(b => b.BeginDate >= DateTime.Now).OrderBy(b => b.BeginDate).FirstOrDefault(); 
        }

        public Booking GetCurrentBooking()
        {
            var books = database.Table<Booking>().FirstOrDefault();
            return database.Table<Booking>().Where(b => b.EndDate <= DateTime.Now).OrderBy(b => b.EndDate).FirstOrDefault();
        }

        public void AddBooking(Booking booking)
        {
            database.InsertOrReplace(booking);
        }

        public void SaveOptions(CoworkingSearchOptions options)
        {
            database.DeleteAll<CoworkingSearchOptions>();
            database.InsertOrReplace(options);
        }

        public CoworkingSearchOptions GetOptions()
        {
            var options = database.Table<CoworkingSearchOptions>().FirstOrDefault();
            if (options == null)
            {
                options = new CoworkingSearchOptions();
            }
            return options;
        }

    }
}
