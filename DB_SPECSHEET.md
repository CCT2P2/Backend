# DB API specsheet
**Time: **11-03-2024 @ 16:42  
**Author:** Sebastian Lindau-Skands @ GNUF | Backend  
**Reciever:** GNUF | DB Team  
**Version:** 0.82, draft  

## Overview
**DB:** sqlite  
**API:** C# (IMC for frequent values)  
**Env:** alpine/sqlite:3.48.0  
**Dedicated com-port for API:** 9012  
**API Auth Method:** Token (64-128bit)  

## Database Schema
  ### Community
  ```
  Column name    Type       Constraints                           Description
  -------------- ---------- ------------------------------------- --------------------------------------
  ID             UUID       PRIMARY KEY                           Unique Identifier
  NAME           TEXT       UNIQUE, NOT NULL                      Community Name
  Description    TEXT       CHECK (LENGTH(description) < 2000)    Community description
  IMG_PATH       TEXT                                             URL to community image
  MEMBER_COUNT   INT        DEFAULT 0, NOT NULL                   Number of members
  TAGS           UUID []    NOT NULL                              Content tags
  POST_IDs       UUID []                                          Array of post ID's (FK. to Posts.ID)
  ```

### User
  ```
  Column Name     Type       Constraints               Description
  --------------- ---------- ------------------------- ---------------------------------------------
  ID              UUID       PRIMARY KEY               Unique Identifier
  EMAIL           TEXT       UNIQUE, NOT NULL          User's Email
  USERNAME        TEXT       UNIQUE, NOT NULL          Unique Username
  PASSWORD        TEXT       NOT NULL                  Hashed Password (SHA256)
  IMG_PATH        TEXT                                 URL TO PP
  POST_IDs        UUID []                              Array of post IDs (FK. to Posts.ID)
  COMMUNITY_IDs   UUID []                              Array of communities (FK. to Communities.ID)
  ADMIN           BOOLEAN    NOT NULL, DEFAULT FALSE   ADMIN FLAG
  TAGS            UUID []                              Array of tags for content recommendation
  ```

### Posts
  ```
  Column Name    Type        Constraints                        Description
  -------------- ----------- ---------------------------------- ---------------------------------------------------------
  POST_ID        UUID        PRIMARY KEY                        Unique Identifier
  TITLE          TEXT        NOT NULL, CHECK (LENGTH < 1000)    Post title
  MAIN_TEXT      TEXT        CHECK (LENGTH < 100k)              Post body
  AUTH_ID        UUID        NOT NULL                           Author ID (FK. User.ID)
  COM_ID         UUID        NOT NULL                           Community ID (FK. Community.ID)
  TIMESTAMP      TIMESTAMP   NOT NULL                           Time of post
  LIKES          INT         NOT NULL, DEFAULT 0                Likes on post
  DISLIKES       INT         NOT NULL, DEFAULT 0                Dislikes on post
  POST_ID_REF    UUID                                           Refference to original post (for reposts) (FK. Post.ID)
  COMMENT_FLAG   BOOLEAN     NOT NULL                           Indicates comment instead of post
  COMMENT_CNT    INT         NOT NULL, DEFAULT 0                Comment count
  Comments       UUID []                                        Array of post id's for comments (FK. Post.ID)
  ```

## API Endpoints
### Authentication
#### User Login
Endpoint: `POST /api/auth/login`  
Desc: Auth user and return access token  
Request body:
  ```json
    {
      "username": "string",
      "password": "string"
    }
  ```
Response:
if success:
  ```json
    {
      "token": "string",
      "user_id": "UUID"
    }
  ```
if not:
  `401 (unauthorised access)`

#### User Registration
Endpoint: `POST /api/auth/register`  
Desc. Register a new user, and return access token  
Request body:
  ```json
    {
      "email": "string",
      "username": "string",
      "password": "string"
    }
  ```
Response:
  ```json
    {
      "token": "string",
      "user_id": "UUID"
    }
  ```
### User Management
#### Get User Profile  
Endpoint: `GET /api/user/{user_id}`  
Desc: Retrieve user data  
Response:
```json
  {
    "id": "UUID",
    "email": "string",
    "username": "string",
    "img_path": "string",
    "post_ids": ["UUID"],
    "community_ids": ["UUID"],
    "tags": ["UUID"],
    "admin": "boolean"
  }
```
#### Delete User Account
Endpoint: `DELETE /api/user/{user_id}`  
desc: Deletes the account  
Response: `204 (No content)`

#### Update User Profile
Endpoint: `PUT /api/user/{user_id}`  
desc: Updates user profile  
Request body:
```json
  {
    "img_path": "string",
    "password": "string",
    "communities": ["UUID"]
  }
```
Response: `200 (OK)`
