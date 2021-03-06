﻿using System;
using StackExchange.Redis;

namespace RedisTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = new RedisConnectionFactory().GetRedisInstance;

            //取得資料庫
            IDatabase db = redis.GetDatabase(1);

            //================================================================================================
            //String , 最基本的型別,最常見的兩種使用方式 1.直接存字串,2.物件序列成化為字串存入
            //================================================================================================
            //Set
            string value = "test:neil";
            db.StringSet("TEST:STRING", "test");

            //Get
            string v = db.StringGet("neil");
            Console.WriteLine(v); // writes: "abcdefg"

            //================================================================================================
            //Hash 是由field和關聯的value組成的map, 適用在只取一個欄位資料，像是氣象資料只要取出溫度欄位，不用整個取出
            //================================================================================================
            //Set
            var hashEntry = new HashEntry[3];
            hashEntry[0] = new HashEntry("Name","Neil");
            hashEntry[1] = new HashEntry("Id","001");
            hashEntry[2] = new HashEntry("Gender","male");
            db.HashSetAsync("TEST:HASH",hashEntry);

            //Get
            var hash = db.HashValues("TEST:HASH");
            Console.WriteLine(hash[1]);
            //================================================================================================

            //================================================================================================
            // Expire 
            //================================================================================================
            //加入expire date 
            db.KeyExpireAsync("TEST:HASH", DateTime.UtcNow.AddMinutes(1));
            //================================================================================================

            //================================================================================================
            //Lists 跟Queue有點像，可用LPush,LPop或是RPush,RPop
            //================================================================================================
            //Left Push
            for (int i = 0; i < 10; i++)
            {
                db.ListLeftPush("TEST:LIST", i);
            }
            //Left Pop
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine(db.ListLeftPop("TEST:LIST"));
            }
            //================================================================================================

            //================================================================================================
            //Sets 是由字串元素組成的一個組合
            //================================================================================================
            //Set
            db.SetAdd("TEST:SET", "NEIL2");
            
            //Get
            Console.WriteLine(db.SetPop("TEST:SET"));
            //================================================================================================

            //================================================================================================
            // Sorted Sets 是由字串元素組成一個集合，而每個元素都會關聯到一個浮動，適用在像是計數器或是排行榜上
            //================================================================================================
            db.SortedSetAdd("TEST:SORTED", "test", 9);
            //================================================================================================

            //================================================================================================
            //Pub/Sub
            //================================================================================================
            //Sub
            ISubscriber sub = redis.GetSubscriber();
            sub.Subscribe("message", (channel, message) =>
            {
                Console.WriteLine((string)message);
            });

            //Pub
            sub.Publish("message", "Hello World");
            //================================================================================================

            Console.ReadLine();
        }
    }
}
