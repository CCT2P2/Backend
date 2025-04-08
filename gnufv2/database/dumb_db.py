import sqlite3
import sys

def show_all_content(db_path):
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    # Get all table names
    cursor.execute("SELECT name FROM sqlite_master WHERE type='table';")
    tables = cursor.fetchall()

    for (table_name,) in tables:
        print(f"\nTable: {table_name}")
        try:
            cursor.execute(f"SELECT * FROM {table_name}")
            rows = cursor.fetchall()

            # Get column names
            column_names = [description[0] for description in cursor.description]
            print(" | ".join(column_names))
            print("-" * 50)

            for row in rows:
                print(" | ".join(str(item) for item in row))
        except sqlite3.Error as e:
            print(f"Failed to read table {table_name}: {e}")

    conn.close()

# Usage example
if __name__ == "__main__":
    show_all_content(sys.argv[1])
