# DB API specsheet  
**Time:** 12-03-2025 @ 11:15
**Author:** Sebastian Lindau-Skands @ GNUF | Backend  
**Reciever:** GNUF | DB Team  
**Version:** 1.2

# Overview  
**DB:** sqlite  
**API:** C# (IMC for frequent values)  
**Env:** alpine/sqlite:3.48.0  
**Dedicated com-port for API:** 9012  
**API Auth Method:** Token (64-128bit)  

# Database Schema
  ## Community
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

## User
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

## Posts
  ```
  Column Name    Type        Constraints                        Description
  -------------- ----------- ---------------------------------- ---------------------------------------------------------
  POST_ID        UUID        PRIMARY KEY                        Unique Identifier
  TITLE          TEXT        NOT NULL, CHECK (LENGTH < 1000)    Post title (if comment, title should be "cmt_{post_id}")
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

# API Endpoints
**Response:** `204 (no content)` if entry not found  
## Authentication
_token generation will be handled by backend, and matched cookie with user UUID no client side_
### User Login
**Endpoint:** `POST /api/auth/login`  
**Desc:** Auth user and return access token  
**Request body:** 
  ```json
    {
      "username": "string",
      "password": "string"
    }
  ```
**Response:**
_if success:_
  ```json
    {
      "user_id": "UUID"
    }
  ```
_if not:_
  `401 (unauthorised access)`

### User Registration
**Endpoint:** `POST /api/auth/register`  
**Desc:** Register a new user, and return access token  
**Request body:**
  ```json
    {
      "email": "string",
      "username": "string",
      "password": "string"
    }
  ```
**Response:**
  ```json
    {
      "user_id": "UUID"
    }
  ```
## User Management
### Get User Profile
**Endpoint:** `GET /api/user/{user_id}`  
**Desc:** Retrieve user data  
**Response:**
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
### Delete User Account
**Endpoint:** `DELETE /api/user/{user_id}`  
**desc:** Deletes the account  
**Response:** `200 (ok)`  

### Update User Profile
**Endpoint:** `PUT /api/user/{user_id}`  
**desc:** Updates user profile  
**Request body:**  
```json
  {
    "img_path": "string",
    "password": "string",
    "communities": ["UUID"]
  }
```
**Response:** `200 (OK)`

## Community management
### Create community
**Endpoint:** `POST /api/community`  
**desc:** Creates a community  
**Request body:**  
```json
  {
    "name": "string",
    "description": "string",
    "img_path": "string",
    "tags": ["UUID"]
  }
```

**Response:**
```json
  {
    "community_id": "UUID"
  }
```

### Get community
**Endpoint:** `GET /api/community/{community_id}`  
**desc:** Fetches details of community  
**Response:**  
```json
  {
    "id": "UUID",
    "name": "string",
    "description": "string",
    "img_path": "string",
    "member_count": "int",
    "tags": ["UUID"],
    "post_ids": ["UUID"]
  }
```

### Update community details
**Endpoint:** `PUT /api/community/{community_id}`  
**desc:** updates details and image of community (not membercount)  
**Request body:**  
```json
  {
    "description": "string",
    "img_path": "string"
  }
```

**Response:** `200 (ok)`

### Update community member
**Endpoint:** `PUT /api/community/{community_id}`  
**desc:** Updates member count of community  
**Request body:**  
```json
  {
    "member_count": "int"
  }
```

**Response:** `200 (ok)`

### Delete community
**Endpoint:** `DELETE /api/community/{community_id}`  
**desc:** deletes the community  
**Response:** `200 (ok)`  

## Post management
### Create post
**Endpoint:** `POST /api/post`  
**desc:** Create a new post  
**Request body:**  
```json
  {
    "title": "string",
    "main_text": "string",
    "auth_id": "UUID",
    "com_id": "UUID",
    "post_id_ref": "UUID (optional)",
    "comment_flag": "boolean"
  }
```
**Response:**
```json
  {
    "post_id": "UUID"
  }
```

### Get post
**Endpoint:** `GET /api/post/{post_id}`  
**desc:** fetch all details about post  
**Response:**  
```json
  {
    "id": "UUID",
    "title": "string",
    "main_text": "string",
    "auth_id": "UUID",
    "com_id": "UUID",
    "timestamp": "timestamp",
    "likes": "int",
    "dislikes": "int",
    "post_id_ref": "UUID",
    "comment_flag": "boolean",
    "comment_count": "int",
    "comments": ["UUID"]
  }
```

### Update post
**Endpoint:** `PUT /api/post/{post_id}`  
**desc:** Edits post  
**Request:**
```json
  {
    "title": "string",
    "main_text": "string",
  }
```

### Delete post
**Endpoint:** `DELETE /api/post/{post_id}`  
**Desc:** Deletes post  
**Response:** `200 (ok)`  

## Interactions
### Like / Dislike
**Endpoint:** `PUT /api/post/{post_id}`  
**Desc:** allows backend to modify like/dislike  
**Request body:**
```json
  {
    "likes": "int",
    "dislikes": "int"
  }
```

**Response:** `200 (ok)`

### Comments 
**Endpoint:** `PUT /api/post/{post_id}`  
**Desc:** Adds ref to comment post to origin post  
**Request body:**
```json
  {
    "Comments": "UUID[]"
  }
```
**Response:** `200 (ok)`

## Search
Search flags and filters are handled backend  
### Search Posts
**Endpoint:** `GET /api/search/posts`  
**Desc:** searches accross all posts  
**Query parameter:** `?q={keyword}`  
**Response:**
```json
  {
    "results": [
      {
        "post_id": "UUID",
        "title": "string",
        "main_text": "string",
        "timestamp": "timestamp"
      }
    ]
  }
```

### Search Communities
**Endpoint:** `GET /api/search/communities`  
**Desc:** searches accross all communities  
**Query parameter:** `q?={keyword}`  
**Response:**
```json
  {
    "results": [
      {
        "community_id": "UUID",
        "name": "string",
        "description": "string",
        "IMG_PATH": "string"
      }
    ]
  }
```

### Search user
**Endpoint:** `GET /api/search/users`  
**Desc:** Searches across all users  
**Query parameter:** `q?={keyword}`  
**Response:**
```json
  {
    "results": [
      {
        "user_id": "UUID",
        "username": "string",
        "IMG_PATH": "string"
      }
    ]
  }
```

# Error handling
## HTTP codes used
- **200 (ok)** | generic response when no response body is required
- **204 (no content)** | use when data entry is empty (so does exist, but only contains NULL)
- **400 (bad request)** | use when request body is malformed
- **401 (unauthorised)** | use when user is a) not found, or b) does not have privilege to perform action
- **404 (not found)** | use when data entry not found
- **500 (internal server error)** | use for all other exceptions

## Security
- Tokens are generated backend, and stored temporarilæy till their expiration time (3600). Tokens are linked to user id's (stored locally in browser cookie) to validate session

## Versioning
Versioning will be done via headers in suggested formmat
```
Description
Date of last edit @ time of last edit
project name @ current version [stable / not stable / not tested]

Responsible editor @ Responsible team
```

# Closing remarks
Due to the extensive nature of the API, DB- and BE team should be cooperating on making it a reality
