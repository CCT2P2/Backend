using System.Data;
using Microsoft.Data.Sqlite;

namespace database;

public class Database
{
    private SqliteConnection _connection;

    public Database()
    {
        _connection = new SqliteConnection("Data Source=src/GNUF.sqlite;");
        OpenConnection();
    }

    private void CheckConnection()
    {
        if (_connection.State == ConnectionState.Open)
        {
            Console.WriteLine("database is open");
        }
        else
        {
            Console.WriteLine("connection is closed\nconnecting to database");
            OpenConnection();
        }
    }

    private void OpenConnection()
    {
        _connection.Open();
        if (_connection.State == ConnectionState.Open)
        {
            Console.WriteLine("Connected to database successfully");
        }
        else
        {
            Console.WriteLine("Failed to connect to database");
        }
    }

    public int? LoginUser(string username, string password)
    {
        CheckConnection();

        SqliteCommand command = new SqliteCommand("SELECT * FROM USER WHERE USER_NAME = @username", _connection);
        command.Parameters.AddWithValue("@username", username);
        SqliteDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            if (password == reader.GetString(3))
            {
                return reader.GetInt32(0);
            }
            else
            {
                return 401;
            }
        }

        return null;
    }


    public int? RegisterUser(string email, string username, string password)
    //Registered user does not use first available ID, meaning if user.id 1 deletes account, user.id 1 will never be used again
    {
        CheckConnection();

        if (LoginUser(username, "dummy") != null)
        {
            return 204;
        }
        Int32 id = GetUserIDs();
        SqliteCommand command = new SqliteCommand("INSERT INTO USER (EMAIL, USER_NAME, PASSWORD) VALUES (@email, @username, @password)", _connection);
        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@username", username);
        command.Parameters.AddWithValue("@password", password);

        if (command.ExecuteNonQuery() > 0)
        {
            return id;
        }
        else
        {
            return null;
        }
    }

    public string? GetUser(int id)
    {
        CheckConnection();

        SqliteCommand command = new SqliteCommand("SELECT * FROM USER WHERE USER_ID = @id", _connection);
        command.Parameters.AddWithValue("@id", id);

        SqliteDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            string email = reader.GetString(1);
            string username = reader.GetString(2);
            string? img_path = reader.GetString(4); //
            string? post_IDS = reader.GetString(5); //
            string? COMM_IDS = reader.GetString(6); //
            string? tags = reader.GetString(8); //
            bool admin = reader.GetBoolean(7);

            return $"{{USER_ID: {id}EMAIL: {email}, USERNAME: {username}, IMG_PATH: {img_path}, POST_IDS: {post_IDS}, COMM_IDS: {COMM_IDS}, tags: {tags}, admin: {admin} }}";
        }

        return null;
    }

    public int? DeleteUser(int? id)
    {
        CheckConnection();

        SqliteCommand command = new SqliteCommand("DELETE FROM USER WHERE USER_ID = @id", _connection);
        command.Parameters.AddWithValue("@id", id);

        if (command.ExecuteNonQuery() > 0)
        {
            return 200;
        }
        return 204;
    }

    public int UpdateUser(int id, string img_path, string password)
    {
        CheckConnection();

        SqliteCommand command =
            new SqliteCommand("UPDATE USER SET PASSWORD = @password, IMAGE_PATH = @img_path WHERE USER_ID = @id",
                _connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@password", password);
        command.Parameters.AddWithValue("@img_path", img_path);

        if (command.ExecuteNonQuery() > 0)
        {
            return 200;
        }
        return 204;
    }

    public int DeletePost(int id)
    {
        CheckConnection();

        SqliteCommand command = new SqliteCommand("DELETE FROM POSTS WHERE POST_ID = @id", _connection);
        command.Parameters.AddWithValue("@id", id);

        if (command.ExecuteNonQuery() > 0)
        {
            return 200;
        }
        return 204;
    }
    public string GetPost(int id)
    {
        CheckConnection();

        SqliteCommand command = new SqliteCommand("SELECT * FROM POSTS WHERE POST_ID = @id", _connection);
        command.Parameters.AddWithValue("@id", id);
        SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            int postId = reader.GetInt32(reader.GetOrdinal("POST_ID"));
            string title = reader.GetString(reader.GetOrdinal("TITLE"));
            string content = reader.GetString(reader.GetOrdinal("MAIN"));
            int authorId = reader.GetInt32(reader.GetOrdinal("AUTHOR_ID"));
            int commId = reader.GetInt32(reader.GetOrdinal("COMMUNITY_ID"));
            int timestamp = reader.GetInt32(reader.GetOrdinal("TIMESTAMP"));
            int likes = reader.GetInt32(reader.GetOrdinal("LIKES"));
            int dislikes = reader.GetInt32(reader.GetOrdinal("DISLIKES"));

            // Handle nullable 'POST_ID_REF'
            int? postIdRef = reader.IsDBNull(reader.GetOrdinal("POST_ID_REF"))
                ? (int?)null
                : reader.GetInt32(reader.GetOrdinal("POST_ID_REF"));

            bool comment = reader.GetBoolean(reader.GetOrdinal("COMMENT_FLAG"));
            int commentCount = reader.GetInt32(reader.GetOrdinal("COMMENT_CNT"));


            return $"{{ID: {postId}, Title: {title}, Content: {content}, author_ID: {authorId} Community_id: {commId} Timestamp: {timestamp} likes: {likes} dislikes: {dislikes} post_id_ref: {postIdRef}, comment: {comment} Comment_cnt: {commentCount}}}";

        }

        reader.Close();
        return null;
    }

    public int updatePostUser(int post_id, string title, string main)
    {
        CheckConnection();

        SqliteCommand command = new SqliteCommand("UPDATE POSTS SET MAIN = @main, TITLE = @title  WHERE POST_ID = @id LIMIT 1", _connection);
        command.Parameters.AddWithValue("@id", post_id);
        command.Parameters.AddWithValue("@main", main);
        command.Parameters.AddWithValue("@title", title);

        if (command.ExecuteNonQuery() > 0)
        {
            return 200;
        }
        return 204;
    }

    public int updatePostBackend(int post_id, int comment_count, string comments, int likes, int dislikes) //potential deletion if proposal 2.B goes through
    {
        CheckConnection();

        SqliteCommand command = new SqliteCommand("UPDATE POSTS SET COMMENT_CNT = @comment_count, LIKES = @likes, DISLIKES = @dislikes, COMMENTS = @comments  WHERE POST_ID = @id LIMIT 1", _connection);
        command.Parameters.AddWithValue("@id", post_id);
        command.Parameters.AddWithValue("@comment_count", comment_count);
        command.Parameters.AddWithValue("@likes", likes);
        command.Parameters.AddWithValue("@dislikes", dislikes);
        command.Parameters.AddWithValue("@comments", comments);

        if (command.ExecuteNonQuery() > 0)
        {
            return 200;
        }
        return 204;
    }

    public int CreatePost(string name, string main, int authID, int commID, int? postIdRef, bool comment)
    {
        CheckConnection();

        int id = GetPostIDs(); // Ensure this generates a unique ID

        SqliteCommand command =
            new SqliteCommand(
                "INSERT INTO POSTS (POST_ID, TITLE, MAIN, AUTHOR_ID, COMMUNITY_ID, POST_ID_REF, COMMENT_FLAG, TIMESTAMP) VALUES (@id, @title, @main, @authID, @commID, @postIDRef, @comment, @time)",
                _connection);

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@title", name);
        command.Parameters.AddWithValue("@main", main);
        command.Parameters.AddWithValue("@authID", authID);
        command.Parameters.AddWithValue("@commID", commID);

        // Correct handling of nullable values
        command.Parameters.AddWithValue("@postIDRef", postIdRef.HasValue ? (object)postIdRef.Value : DBNull.Value);
        command.Parameters.AddWithValue("@comment", comment);

        // Store timestamp as an integer (Unix Time)
        command.Parameters.AddWithValue("@time", (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        int rowsAffected = command.ExecuteNonQuery();
        if (rowsAffected > 0)
        {
            return id; // Return the generated post ID
        }

        return 500; // Return 500 if the insert fails
    }
    public int CreateCommunity(string communityName, string communityDescription, string imagePath, int memberCount, string tags, int? postIds)
    {
        CheckConnection();

        int id = GetCommIDs(); // Ensure this generates a unique ID

        SqliteCommand command =
            new SqliteCommand(
            "INSERT INTO COMMUNITY (COMMUNITY_ID, COMMUNITY_NAME, COMMUNITY_DESCRIPTION, IMAGE_PATH, MEMBER_CNT, TAGS, POST_IDs) VALUES (@id, @communityName, @communityDescription, @imagePath, @memberCount, @tags, @postIds)",
            _connection);

        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@communityName", communityName);
        command.Parameters.AddWithValue("@communityDescription", communityDescription);
        command.Parameters.AddWithValue("@imagePath", imagePath);
        command.Parameters.AddWithValue("@memberCount", memberCount);
        command.Parameters.AddWithValue("@tags", tags);

        // Correct handling of nullable values
        command.Parameters.AddWithValue("@postIds", postIds.HasValue ? (object)postIds.Value : DBNull.Value);

        int rowsAffected = command.ExecuteNonQuery();
        if (rowsAffected > 0)
        {
            return id; // Return the generated post ID
        }

        return 500; // Return 500 if the insert fails
    }

    public string GetCommunity(int id)
    {
        CheckConnection();

        SqliteCommand command = new SqliteCommand("SELECT * FROM COMMUNITY WHERE COMMUNITY_ID = @id", _connection);
        command.Parameters.AddWithValue("@id", id);
        SqliteDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            string communityName = reader.GetString(1);
            string? communityDescription = reader.GetString(2);
            string? img_path = reader.GetString(3);
            int memberCount = reader.GetInt32(4);
            string tags = reader.GetString(5);
            string? post_IDS = reader.GetString(6);

            return ($"ID: {id}, community name: {communityName}, Community description: {communityDescription}, Image Path: {img_path}, Member count: {memberCount}, Tags: {tags}, Post IDS: {post_IDS}");
        }

        return null;
    }

    public int UpdateCommunity_User(int id, string community_name, string community_description, string img_path, string tags)
    {
        CheckConnection();

        SqliteCommand command =
            new SqliteCommand("UPDATE COMMUNITY SET COMMUNITY_NAME = @community_name, COMMUNITY_DESCRIPTION = @community_description, IMAGE_PATH = @img_path, TAGS = @tags WHERE COMMUNITY_ID = @id",
                _connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@community_name", community_name);
        command.Parameters.AddWithValue("@community_description", community_description);
        command.Parameters.AddWithValue("@img_path", img_path);
        command.Parameters.AddWithValue("@tags", tags);

        if (command.ExecuteNonQuery() > 0)
        {
            return 200;
        }
        return 204;
    }
    public int DeleteCommunity(int id)
    {
        CheckConnection();

        SqliteCommand command = new SqliteCommand("DELETE FROM COMMUNITY WHERE COMMUNITY_ID = @id", _connection);
        command.Parameters.AddWithValue("@id", id);

        if (command.ExecuteNonQuery() > 0)
        {
            return 200;
        }
        return 204;
    }

    public int UpdateCommunity_Backend(int id, int memberCount, string tags, string post_id) //potential deletion if proposal 2.B goes through
    {
        CheckConnection();

        SqliteCommand command =
            new SqliteCommand("UPDATE COMMUNITY SET MEMBER_CNT = @memberCount, TAGS = @tags, POST_IDs= @post_id WHERE COMMUNITY_ID = @id", _connection);
        command.Parameters.AddWithValue("@memberCount", memberCount);
        command.Parameters.AddWithValue("@tags", tags);
        command.Parameters.AddWithValue("@post_id", post_id);
        command.Parameters.AddWithValue("@id", id);


        if (command.ExecuteNonQuery() > 0)
        {
            return 200;
        }
        return 204;
    }

    public int LikeDislike_Count(int post_id, int like_count, int dislike_count)
    {
        CheckConnection();

        SqliteCommand command = new SqliteCommand("UPDATE POST SET LIKE_COUNT =@like, DISLIKE_COUNT =@dislike, WHERE POST_ID= @post_id", _connection);


        command.Parameters.AddWithValue("@like", like_count);
        command.Parameters.AddWithValue("@dislike", dislike_count);
        command.Parameters.AddWithValue("@post_id", post_id);


        if (command.ExecuteNonQuery() > 0)
        {
            return 200;
        }
        return 204;
    }

    private int GetUserIDs()
    {
        CheckConnection();

        List<int> ids = new List<int>();
        SqliteCommand command = new SqliteCommand("SELECT * FROM USER", _connection);
        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            ids.Add(reader.GetInt32(0));
        }

        int temp = 0;
        if (ids.Count == 0)
        {
            throw new Exception("No users found in the database.");
        }
        foreach (int id in ids)
        {
            if (id == temp + 1)
            {
                temp = id;
            }
            else
            {
                break;
            }
        }
        return temp + 1;
    }

    public int GetPostIDs()
    {
        CheckConnection();

        List<int> ids = new List<int>();
        SqliteCommand command = new SqliteCommand("SELECT * FROM POSTS", _connection);
        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            ids.Add(reader.GetInt32(0));
        }

        int temp = 100;
        if (ids.Count == 0)
        {
            throw new Exception("No users found in the database.");
        }
        foreach (int id in ids)
        {
            if (id == temp + 1)
            {
                temp = id;

            }
            else
            {
                break;
            }
        }
        return temp + 1;
    }

    private int GetCommIDs()
    {
        CheckConnection();

        List<int> ids = new List<int>();
        SqliteCommand command = new SqliteCommand("SELECT * FROM COMMUNITY", _connection);
        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            ids.Add(reader.GetInt32(0));
        }

        int temp = 0;
        if (ids.Count == 0)
        {
            throw new Exception("No communities found in the database.");
        }
        foreach (int id in ids)
        {
            if (id == temp + 1)
            {
                temp = id;
            }
            else
            {
                break;
            }
        }
        return temp + 1;
    }

}
