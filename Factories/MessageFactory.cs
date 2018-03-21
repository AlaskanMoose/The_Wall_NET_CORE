using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using TheWall.Models;
using static Dapper.SqlMapper;

namespace TheWall.Factories{
  public class MessageFactory : IFactory<Message>{
    private readonly IOptions<MySqlOptions> MySqlConfig;

    public MessageFactory(IOptions<MySqlOptions> config){
        MySqlConfig = config;
    }

    internal IDbConnection Connection{
        get { return new MySqlConnection(MySqlConfig.Value.ConnectionString);}
    }
    public void Add(Message Item){
      using(IDbConnection dbConnection = Connection){
        string Query = "INSERT INTO Messages (MessageContent, user_id, CreatedAt) VALUES (@MessageContent, @user_id, NOW())";
        dbConnection.Open();
        dbConnection.Execute(Query, Item);
      }
    }

    public List<Message> GetAllMessages(){
      using(IDbConnection dbConnection = Connection){
        string Query = @"
                SELECT * FROM Messages JOIN users ON Messages.user_id WHERE Messages.user_id = Users.UserId ORDER BY Messages.CreatedAt DESC; SELECT * FROM Comments JOIN Users ON Comments.user_id WHERE Comments.user_id = Users.UserId";
        dbConnection.Open();
        using(GridReader multi = dbConnection.QueryMultiple(Query, null))
        {

            List<Message> Messages = multi.Read<Message, User, Message>((message, user) => { message.Poster = user; return message; }, splitOn: "UserId").ToList();

            List<Comment> Comments = multi.Read<Comment, User, Comment>((comment, user) => { comment.Commenter = user; return comment; }, splitOn: "UserId").ToList();

            List<Message> combo = Messages.GroupJoin(
                        Comments,
                        message => message.MessageId,
                        comment => comment.message_id,
                        (message, comments) =>
                        {
                            message.Comments = comments.ToList();
                            return message;
                        }).ToList();

            return combo;
        }
      }
    }

    public Message GetMessageById(int Id)
    {
        using(IDbConnection dbConnection = Connection)
        {
            string Query = $"SELECT * FROM Messages WHERE MessageId = {Id} LIMIT 1";

            dbConnection.Open();
            // dbConnection.Query<Message, User, Message>(Query, ());
            return dbConnection.QuerySingleOrDefault<Message>(Query);
        }
    }

    public void DeleteMessage(int Id)
    {
        using(IDbConnection dbConnection = Connection)
        {
            string Query = $"DELETE FROM Messages WHERE MessageId = {Id}";

            dbConnection.Open();
            dbConnection.Execute(Query);
        }
    }


  }
}