import sqlite3
import json

def parse_uuid_array(uuid_array_string):
    """Parses a UUID[] string into a Python list."""
    if not uuid_array_string:
        return []

    if isinstance(uuid_array_string, list):
        return uuid_array_string

    if isinstance(uuid_array_string, str):
        try:
            # Try JSON-style
            return json.loads(uuid_array_string)
        except json.JSONDecodeError:
            # Fallback: PostgreSQL-style UUID[] like '{id1,id2}'
            uuid_array_string = uuid_array_string.strip('{}')
            if uuid_array_string == '':
                return []
            return [x.strip() for x in uuid_array_string.split(',') if x.strip()]

    return []

def get_top_active_users(db_path="GNUF.sqlite", limit=10):
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    try:
        cursor.execute("SELECT USER_ID, USERNAME, POST_IDs FROM USER")
        users = cursor.fetchall()

        user_post_counts = []
        for user_id, username, post_ids_raw in users:
            parsed_ids = parse_uuid_array(post_ids_raw)
            if not isinstance(parsed_ids, list):
                print(f"Warning: Skipping user '{username}' due to malformed POST_IDs")
                continue
            user_post_counts.append((user_id, username, len(parsed_ids)))

        # Sort and get top N
        top_users = sorted(user_post_counts, key=lambda x: x[2], reverse=True)[:limit]

        print("Top 10 Most Active Users (by number of posts):")
        for rank, (user_id, username, count) in enumerate(top_users, start=1):
            print(f"{rank}. {username} (ID: {user_id}) — {count} posts")

    except sqlite3.Error as e:
        print("SQLite error:", e)
    finally:
        conn.close()

if __name__ == "__main__":
    get_top_active_users()

