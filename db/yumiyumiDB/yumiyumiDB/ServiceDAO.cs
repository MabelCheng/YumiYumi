﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace yumiyumiDB
{
    public class ServiceDAO
    {
        public bool addOneService(OrderEntity order)
        {
            string mysql = "insert `yumi_service`(`id`,`user_id`,`restaurant_id`,`service_type`,`ctime`) values(5,1,1,2,'2008-9-4 0:00:00');";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?userId", MySqlDbType.Int32),
                    new MySqlParameter("?restaurantId", MySqlDbType.Int32),
                    new MySqlParameter("?remark", MySqlDbType.VarChar,255),
                    new MySqlParameter("?total", MySqlDbType.Int32),
                    new MySqlParameter("?status", MySqlDbType.Int32)
                    };
            parameters[0].Value = order.user_id;
            parameters[1].Value = order.restaurant_id;
            parameters[2].Value = order.remark;
            parameters[3].Value = order.total_price;
            parameters[4].Value = order.status;

            //通过MySqlCommand的ExecuteReader()方法构造DataReader对象
            int count = MySqlHelper.ExecuteNonQuery(mysql, parameters);
            if (count > 0) { return true; }
            else { return false; }
        }

        public bool cancleById(int serviceId)
        {
            string mysql = "UPDATE yumi_service SET status = '2' WHERE id = ?serviceId";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?serviceId", MySqlDbType.UInt32),
                    };
            parameters[0].Value = serviceId;
            int count = MySqlHelper.ExecuteNonQuery(mysql, parameters);
            if (count > 0) { return true; }
            else { return false; }
        }

        public bool finishService(int serviceId)
        {
            string mysql = "UPDATE yumi_service SET status = '1' WHERE id = ?serviceId";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?serviceId", MySqlDbType.UInt32),
                    };
            parameters[0].Value = serviceId;
            int count = MySqlHelper.ExecuteNonQuery(mysql, parameters);
            if (count > 0) { return true; }
            else { return false; }
        }


        public List<ServiceEntity> getAllServiceByRestaurantId(int restaurantId)
        {
            string mysql = "SELECT `id`,`user_id`,`restaurant_id`,`service_type`,`ctime`,`status` FROM `yumi_service` WHERE `restaurant_id` =?restaurantId";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?restaurantId", MySqlDbType.Int32)
                    };
            parameters[0].Value = restaurantId;
            MySqlDataReader myreader = MySqlHelper.ExecuteReader(mysql, parameters);
            List<ServiceEntity> list = new List<ServiceEntity>();
            while (myreader.Read())
            {
                ServiceEntity service = new ServiceEntity();
                service.id = myreader.GetInt32(0);
                service.user_id = myreader.GetInt32(1);
                service.restaurant_id = myreader.GetInt32(2);
                service.service_type = myreader.GetInt32(3);
                service.ctime = myreader.GetString(4);
                service.status = myreader.GetInt32(5);
                list.Add(service);
            }
            myreader.Close();
            for (int i = 0; i < list.Count;i++ )
            {
                string mysql2 = "SELECT `service_name` FROM `yumi_service_type` WHERE `service_id` =?serviceId";
                MySqlParameter[] parameters2 = {
                    new MySqlParameter("?serviceId", MySqlDbType.Int32)
                    };
                parameters2[0].Value = list[i].service_type;
                MySqlDataReader myreader2 = MySqlHelper.ExecuteReader(mysql2, parameters2);

                while (myreader2.Read())
                {
                    list[i].service_name = myreader2.GetString(0);
                }
                myreader2.Close();
            }
            myreader.Close();
            return list;
        }

        
    }
}