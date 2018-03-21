using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using TheWall.Models;

namespace TheWall.Factories
{
    public class CommentFactory : IFactory<Comment>
    {
        private readonly IOptions<MySqlOptions> MySqlConfig;
        
        public CommentFactory(IOptions<MySqlOptions> config)
        {
            MySqlConfig = config;
        }
        
        internal IDbConnection Connection
        {
            get { return new MySqlConnection(MySqlConfig.Value.ConnectionString);}
        }

        public void Add(Comment Item)
        {
            using(IDbConnection dbConnection = Connection)
            {
                string Query = "INSERT INTO Comments (CommentContent, message_id, user_id, CreatedAt) VALUES (@CommentContent, @message_id, @user_id, NOW())";
                dbConnection.Open();
                dbConnection.Execute(Query, Item);
            }
        }
    }
}