﻿using System;
using System.Collections.Generic;
using MongoDB.Defination;
using MongoDB.Driver;
using MongoDB.Model;

namespace MongoDB.Component
{
    public class MongoCollectionInfo : MongoBaseInfo
    {
        public MongoCollectionInfo(uint id)
        {
            var tbNode = MongoCache.GetTreeNode(id);
            Table = MongoCache.GetMongoObject(id) as MongoCollectionModel;
            var dbNode = MongoCache.GetTreeNode(tbNode.PID);
            Database = MongoCache.GetMongoObject(dbNode.ID) as MongoDatabaseModel;
            var serverNode = MongoCache.GetTreeNode(dbNode.PID);
            Server = MongoCache.GetMongoObject(serverNode.ID) as MongoServerModel;
        }

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <returns></returns>
        public override List<MongoTreeNode> GetInfo()
        {
            var mongo = new MongoClient(string.Format(MongoConst.ConnString, Server.Name));
            var server = mongo.GetServer();
            var db = server.GetDatabase(Database.Name);
            var rst = db.RunCommand(new CommandDocument { { "collstats", Table.Name } });

            var list = new List<MongoTreeNode>();
            if (rst.Ok)
            {
                BuildTreeNode(list, 0, rst.Response);
            }
            return list;
        }
    }
}