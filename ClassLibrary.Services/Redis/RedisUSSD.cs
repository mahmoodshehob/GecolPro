using ClassLibrary.Services.Redis.Model;
using StackExchange.Redis;

namespace ClassLibrary.Services.Redis
{
    public class RedisUSSD
    {
        // Cache Model
        private VarCacheTrans cacheMem;

        // private static ConnectionMultiplexer _Redis;
        private static ConnectionMultiplexer _redis;
        private static string RedisServer = "localhost";
        private static int RedisPort = 6379;



        private static int sessionPerdiod = 50;


        // Redis Connection
        private static void InitializeRadis()
        {
            string HOST_NAME = "127.0.0.1";
            //int PORT_NUMBER = 6379;
            //string PASSWORD = "Rd!8525002";
            //_Redis = ConnectionMultiplexer.Connect(HOST_NAME + ":" + PORT_NUMBER + ",password=" + PASSWORD);

            _redis = ConnectionMultiplexer.Connect(HOST_NAME+":6379,password=Rd!8525002");
        }


        // Redis Disconnection
        private static void CloseRadis()
        {
            _redis.Close();
        }




        //public static void MainRedis()
        //{
        //    do
        //    {
        //        InitializeRadis();
        //    }
        //    while (_redis.IsConnected != true);


        //    IDatabase db = _redis.GetDatabase();

        //    var ret = db.Ping();

        //    VarCacheTrans cacheMem = new VarCacheTrans()
        //    {
        //        TransID = "123456798",
        //        Input = "1",
        //        MenuId = "M0"

        //    };

        //    string JSonCacheMem = Newtonsoft.Json.JsonConvert.SerializeObject(cacheMem);

        //    // db.SetAdd(cacheMem.TransID, new RedisValue[] { cacheMem.TransID, JSonCacheMem });

        //    db.StringSet(cacheMem.TransID, JSonCacheMem, TimeSpan.FromSeconds(20));

        //    VarCacheTrans getObj = Newtonsoft.Json.JsonConvert.DeserializeObject<VarCacheTrans>(db.StringGet(cacheMem.TransID));

        //    CloseRadis();
        //}

        //
        //
        //

        //public static void FirtTimeOrder(VarCacheTrans cacheMem)
        //{
        //    do
        //    {
        //        InitializeRadis();
        //    }
        //    while (_redis.IsConnected != true);

        //    IDatabase db = _redis.GetDatabase();

        //    //Boolean isDeleted = Delete_ID(cacheMem);
        //    Delete_ID(cacheMem);

        //    cacheMem.MenuIdNew = cacheMem.Input.Replace("#", "");

        //    string JSonCacheMem = Newtonsoft.Json.JsonConvert.SerializeObject(cacheMem);

        //    db.StringSet(cacheMem.TransID, JSonCacheMem, TimeSpan.FromSeconds(sessionPerdiod));

        //    //VarCacheTrans getObj = Newtonsoft.Json.JsonConvert.DeserializeObject<VarCacheTrans>(db.StringGet(cacheMem.TransID));
        //    CloseRadis();
        //}

        //
        //
        //

        public static VarCacheTrans QueryNode(string TransID)
        {

            //string JSonCacheMem = Newtonsoft.Json.JsonConvert.SerializeObject(cacheMem);
            VarCacheTrans getObj;
            do
            {
                InitializeRadis();
            }
            while (_redis.IsConnected != true);

            IDatabase db = _redis.GetDatabase();


            if (db.KeyExists(TransID))
            {

                getObj = Newtonsoft.Json.JsonConvert.DeserializeObject<VarCacheTrans>(db.StringGet(TransID));
                db.KeyExpire(TransID, TimeSpan.FromSeconds(sessionPerdiod));
            }
            else
            {
                getObj = null;

            }



            CloseRadis();

            return getObj;
        }

        //
        //
        //

        public static void UpdateNode(VarCacheTrans cacheMem)
        {

            string JSonCacheMem = Newtonsoft.Json.JsonConvert.SerializeObject(cacheMem);

            do
            {
                InitializeRadis();
            }
            while (_redis.IsConnected != true);

            IDatabase db = _redis.GetDatabase();

            db.StringSet(cacheMem.MSISDN, JSonCacheMem, TimeSpan.FromSeconds(sessionPerdiod));

            CloseRadis();
        }

        //
        //
        //

        public static void BackNode(VarCacheTrans cacheMem)
        {

            string JSonCacheMem = Newtonsoft.Json.JsonConvert.SerializeObject(cacheMem);

            do
            {
                InitializeRadis();
            }
            while (_redis.IsConnected != true);

            IDatabase db = _redis.GetDatabase();


            db.StringSet(cacheMem.MSISDN, JSonCacheMem, TimeSpan.FromSeconds(sessionPerdiod));

            CloseRadis();
        }

        //
        //
        //


        public static void Delete_ID(VarCacheTrans cacheMem)
        {
            do
            {
                InitializeRadis();
            }
            while (_redis.IsConnected != true);

            IDatabase db = _redis.GetDatabase();
            Boolean isDeleted = db.KeyDelete(cacheMem.MSISDN);

            CloseRadis();
        }

        //
        //
        //

        public static void PingRedis()
        {
            do
            {
                InitializeRadis();
            }
            while (_redis.IsConnected != true);

            IDatabase db = _redis.GetDatabase();

            var ret = db.Ping();

            CloseRadis();
        }

        //
        //
        //

        //public static async void TestRedis()
        //{
        //    for (int i = 0; i < 100000; i++)
        //    {
        //        Random rnd = new Random();
        //        int num = rnd.Next();
        //        VarCacheTrans cacheMem = new VarCacheTrans()
        //        {
        //            TransID = num.ToString(),
        //            Input = "1",
        //            MenuId = "M0"
        //        };

        //        string JSonCacheMem = Newtonsoft.Json.JsonConvert.SerializeObject(cacheMem);

        //        do
        //        {
        //            InitializeRadis();
        //        }
        //        while (_redis.IsConnected != true);

        //        IDatabase db = _redis.GetDatabase();

        //        //

        //        db.StringSet(cacheMem.TransID, JSonCacheMem, TimeSpan.FromSeconds(50));

        //        //
        //        CloseRadis();
        //    }
        //}

        //
        //
        //

        public static void TestRedisCach(VarCacheTrans cacheMem)
        {

            string JSonCacheMem = Newtonsoft.Json.JsonConvert.SerializeObject(cacheMem);



            do
            {
                InitializeRadis();
            }
            while (_redis.IsConnected != true);


            IDatabase db = _redis.GetDatabase();

            string cont = db.StringGet("1645987");

            var ret = db.Ping();


            db.StringSet(cacheMem.MSISDN, JSonCacheMem, TimeSpan.FromSeconds(50));

            VarCacheTrans getObj = Newtonsoft.Json.JsonConvert.

            DeserializeObject<VarCacheTrans>(db.StringGet(cacheMem.MSISDN));

            CloseRadis();
        }

        //
        //
        //

        //public static void MenuNode(VarCacheTrans cacheMem)
        //{

        //    string JSonCacheMem = Newtonsoft.Json.JsonConvert.SerializeObject(cacheMem);

        //    do
        //    {
        //        InitializeRadis();
        //    }
        //    while (_redis.IsConnected != true);

        //    IDatabase db = _redis.GetDatabase();

        //    VarCacheTrans getObj = Newtonsoft.Json.JsonConvert.DeserializeObject<VarCacheTrans>(db.StringGet(cacheMem.TransID));

        //    if (getObj == null)
        //    {

        //    }

        //    string cont = db.StringGet(cacheMem.TransID);


        //    db.StringSet(cacheMem.TransID, JSonCacheMem, TimeSpan.FromSeconds(50));

        //    VarCacheTrans getObj = Newtonsoft.Json.JsonConvert.
        //        DeserializeObject<VarCacheTrans>(db.StringGet(cacheMem.TransID));




        //    CloseRadis();
        //}

    }
}
