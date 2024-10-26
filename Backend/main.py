from fastapi import FastAPI, HTTPException, Depends, Response
from sqlalchemy import create_engine, Column, Integer, String, Boolean, ForeignKey
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker, Session
from pydantic import BaseModel, ValidationError
from typing import List, Optional
from fastapi.responses import JSONResponse

# Database setup
SQLALCHEMY_DATABASE_URL = "sqlite:///./test.db"
engine = create_engine(SQLALCHEMY_DATABASE_URL)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
Base = declarative_base()

# Models
class User(Base):
    __tablename__ = "users"
    id = Column(Integer, primary_key=True, index=True)
    name = Column(String, index=True)
    is_admin = Column(Boolean, default=False)
    image_url = Column(String)

class Post(Base):
    __tablename__ = "posts"
    id = Column(Integer, primary_key=True, index=True)
    title = Column(String, index=True)
    post_text = Column(String)
    likes = Column(Integer, default=0)
    user_id = Column(Integer, ForeignKey("users.id"))

Base.metadata.create_all(bind=engine)

# Pydantic models
class UserCreate(BaseModel):
    name: str
    is_admin: bool = False
    image_url: str = ""

class UserUpdate(BaseModel):
    name: Optional[str] = None
    is_admin: Optional[bool] = None
    image_url: Optional[str] = None

class UserResponse(BaseModel):
    id: int
    name: str
    is_admin: bool
    image_url: str

class PostCreate(BaseModel):
    title: str
    post_text: str
    user_id: int

class PostUpdate(BaseModel):
    title: Optional[str] = None
    post_text: Optional[str] = None

class PostResponse(BaseModel):
    id: int
    title: str
    post_text: str
    likes: int
    user_id: int

# Dependency
def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

app = FastAPI()

@app.get("/")
def read_root():
    return JSONResponse(content={"message": "Welcome to the API"})

# User endpoints
@app.post("/users/", response_model=UserResponse)
def create_user(user: UserCreate, db: Session = Depends(get_db)):
    try:
        db_user = User(**user.dict())
        db.add(db_user)
        db.commit()
        db.refresh(db_user)
        return db_user
    except Exception as e:
        raise HTTPException(status_code=422, detail=str(e))

@app.get("/users/", response_model=List[UserResponse])
def get_users(name: Optional[str] = None, db: Session = Depends(get_db)):
    query = db.query(User)
    if name:
        query = query.filter(User.name.contains(name))
    return query.all()

@app.get("/users/{user_id}", response_model=UserResponse)
def get_user(user_id: int, db: Session = Depends(get_db)):
    user = db.query(User).filter(User.id == user_id).first()
    if user is None:
        raise HTTPException(status_code=404, detail="User not found")
    return user

@app.put("/users/{user_id}", response_model=UserResponse)
def update_user(user_id: int, user: UserUpdate, db: Session = Depends(get_db)):
    db_user = db.query(User).filter(User.id == user_id).first()
    if db_user is None:
        raise HTTPException(status_code=404, detail="User not found")
    for key, value in user.dict(exclude_unset=True).items():
        setattr(db_user, key, value)
    db.commit()
    db.refresh(db_user)
    return db_user

@app.delete("/users/{user_id}", response_model=UserResponse)
def delete_user(user_id: int, db: Session = Depends(get_db)):
    db_user = db.query(User).filter(User.id == user_id).first()
    if db_user is None:
        raise HTTPException(status_code=404, detail="User not found")
    db.delete(db_user)
    db.commit()
    return db_user

# Post endpoints
@app.post("/posts/", response_model=PostResponse)
def create_post(post: PostCreate, db: Session = Depends(get_db)):
    db_post = Post(**post.dict())
    db.add(db_post)
    db.commit()
    db.refresh(db_post)
    return db_post

@app.get("/posts/", response_model=List[PostResponse])
def get_posts(title: Optional[str] = None, db: Session = Depends(get_db)):
    query = db.query(Post)
    if title:
        query = query.filter(Post.title.contains(title))
    return query.all()

@app.get("/posts/{post_id}", response_model=PostResponse)
def get_post(post_id: int, db: Session = Depends(get_db)):
    post = db.query(Post).filter(Post.id == post_id).first()
    if post is None:
        raise HTTPException(status_code=404, detail="Post not found")
    return post

@app.put("/posts/{post_id}", response_model=PostResponse)
def update_post(post_id: int, post: PostUpdate, db: Session = Depends(get_db)):
    db_post = db.query(Post).filter(Post.id == post_id).first()
    if db_post is None:
        raise HTTPException(status_code=404, detail="Post not found")
    for key, value in post.dict(exclude_unset=True).items():
        setattr(db_post, key, value)
    db.commit()
    db.refresh(db_post)
    return db_post

@app.delete("/posts/{post_id}", response_model=PostResponse)
def delete_post(post_id: int, db: Session = Depends(get_db)):
    db_post = db.query(Post).filter(Post.id == post_id).first()
    if db_post is None:
        raise HTTPException(status_code=404, detail="Post not found")
    db.delete(db_post)
    db.commit()
    return db_post

@app.get("/favicon.ico", include_in_schema=False)
async def favicon():
    return Response(status_code=204)

@app.exception_handler(ValidationError)
async def validation_exception_handler(request, exc):
    return JSONResponse(
        status_code=422,
        content={"detail": str(exc)}
    )

