using System;
using System.Collections.Generic;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace FacebookDeskAppLogic
{
    public sealed class LoggedinUserData
    {
        // Private Members
        private User m_User;
        private IDictionary<string, List<Post>> m_DictionaryOfPostsByPlaces = new Dictionary<string, List<Post>>();
        private IDictionary<string, List<Post>> m_DictionaryOfPostsByLikes = new Dictionary<string, List<Post>>();
        private IDictionary<string, List<Post>> m_DictionaryOfPostsByComments = new Dictionary<string, List<Post>>();
        private IDictionary<string, List<Photo>> m_DictionaryOfPhotosByAlbumName = new Dictionary<string, List<Photo>>();
        private ICollection<Album> m_ListOfAlbums = new List<Album>();
        private ICollection<User> m_ListOfFriends = new List<User>();
        private ICollection<Group> m_ListOfGroups = new List<Group>();

        // Constructors
        private LoggedinUserData()
        {
        }

        // Properties
        public User User
        {
            get
            {
                return m_User;
            }

            set
            {
                m_User = value;
            }
        }

        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        //---------------------------Dictionary methods-------------------------//
        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        private void addElementToDictionaryByKey<T>(string i_Key, IDictionary<string, List<T>> i_Dictionary, T i_Element)
        {
            if (i_Dictionary.ContainsKey(i_Key))
            {
                i_Dictionary[i_Key].Add(i_Element);
            }
            else
            {
                i_Dictionary.Add(i_Key, new List<T>());
                i_Dictionary[i_Key].Add(i_Element);
            }
        }

        private void addPostToDictionaryByPlace(string i_PlaceName, Post i_Post)
        {
            addElementToDictionaryByKey(i_PlaceName, m_DictionaryOfPostsByPlaces, i_Post);
        }

        private void addPostToDictionaryByLikes(Post i_Post)
        {
            try
            {
                int numOfLikes = i_Post.LikedBy.Count;
                addPostToDictionaryByNumericValue(numOfLikes, m_DictionaryOfPostsByLikes, i_Post);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void addPostToDictionaryByNumericValue(int i_NumericValue, IDictionary<string, List<Post>> i_DictionaryOfPosts, Post i_Post)
        {
            if (i_NumericValue >= 1 && i_NumericValue <= 10)
            {
                addElementToDictionaryByKey("1-10", i_DictionaryOfPosts, i_Post);
            }
            else if (i_NumericValue >= 11 && i_NumericValue <= 20)
            {
                addElementToDictionaryByKey("11-20", i_DictionaryOfPosts, i_Post);
            }
            else if (i_NumericValue >= 21 && i_NumericValue <= 50)
            {
                addElementToDictionaryByKey("21-50", i_DictionaryOfPosts, i_Post);
            }
            else if (i_NumericValue >= 51 && i_NumericValue <= 100)
            {
                addElementToDictionaryByKey("51-100", i_DictionaryOfPosts, i_Post);
            }
            else if (i_NumericValue >= 101 && i_NumericValue <= 200)
            {
                addElementToDictionaryByKey("101-200", i_DictionaryOfPosts, i_Post);
            }
            else if (i_NumericValue > 200)
            {
                addElementToDictionaryByKey("Above 200", i_DictionaryOfPosts, i_Post);
            }
        }

        private void addPostToDictionaryByComments(Post i_Post)
        {
            int numOfComments = i_Post.Comments.Count;
            addPostToDictionaryByNumericValue(numOfComments, m_DictionaryOfPostsByComments, i_Post);
        }

        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        //---------------------------Fetch methods------------------------------//
        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        public List<Post> FetchAllPosts()
        {
            FacebookObjectCollection<Post> posts = m_User.Posts;
            List<Post> listOfPosts = new List<Post>();
            foreach (Post post in posts)
            {
                listOfPosts.Add(post);
            }

            return listOfPosts;
        }

        public void FetchPostsByPlaces()
        {
            FacebookObjectCollection<Post> posts = m_User.Posts;
            m_DictionaryOfPostsByPlaces.Clear();

            foreach (Post post in posts)
            {
                Page page = post.Place;
                if (page != null && page.Name != null)
                {
                    addPostToDictionaryByPlace(page.Name, post);
                }
            }
        }

        public void FetchPostsByNumOfLikes()
        {
            FacebookObjectCollection<Post> posts = m_User.Posts;
            m_DictionaryOfPostsByLikes.Clear();
            foreach (Post post in posts)
            {
                addPostToDictionaryByLikes(post);
            }
        }

        public void FetchPostsByNumOfComments()
        {
            FacebookObjectCollection<Post> posts = m_User.Posts;
            m_DictionaryOfPostsByComments.Clear();
            foreach (Post post in posts)
            {
                addPostToDictionaryByComments(post);
            }
        }

        public void FetchGroups()
        {
            foreach (Group group in m_User.Groups)
            {
                m_ListOfGroups.Add(group);
            }
        }

        public void FetchAlbums()
        {
            foreach (Album album in m_User.Albums)
            {
                m_ListOfAlbums.Add(album);
            }
        }

        public List<Photo> FetchPhotosByAlbumName(string i_AlbumName)
        {
            int index = 1;
            List<Photo> listOfPhotos = new List<Photo>();
            try
            {
                foreach(Album album in m_User.Albums)
                {
                    if(album.Name == i_AlbumName)
                    {
                        foreach(Photo photo in album.Photos)
                        {
                            listOfPhotos.Add(photo);
                            index++;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return listOfPhotos;
        }

        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        //-----------------------------Get methods------------------------------//
        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        public List<string> GetPlaceNamesOfPosts()
        {
            List<string> placesNamesList = new List<string>();

            if (m_DictionaryOfPostsByPlaces.Count != 0)
            {
                foreach (KeyValuePair<string, List<Post>> entry in m_DictionaryOfPostsByPlaces)
                {
                    string placeName = entry.Key;
                    placesNamesList.Add(placeName);
                }
            }

            return placesNamesList;
        }

        public ICollection<Post> GetAllPosts()
        {
            if (m_User.Posts.Count != 0)
            {
                return m_User.Posts;
            }
            else
            {
                return null;
            }
        }

        public ICollection<Album> GetAllAlbums()
        {
             return m_ListOfAlbums;
        }

        public ICollection<User> GetAllFriends()
        {
            return m_ListOfFriends;
        }

        public ICollection<Group> GetAllGroups()
        {
            return m_ListOfGroups;
        }

        public ICollection<Post> GetPostsByPlaceName(string i_PlaceName)
        {
            return m_DictionaryOfPostsByPlaces[i_PlaceName];
        }

        public ICollection<Post> GetPostsByNumOfLikes(string i_NumOfLikes)
        {
            if(m_DictionaryOfPhotosByAlbumName.ContainsKey(i_NumOfLikes))
            {
                return m_DictionaryOfPostsByLikes[i_NumOfLikes];
            }
            else
            {
                return null;
            }
        }

        public ICollection<Post> GetPostsByNumOfComments(string i_NumOfComments)
        {
            if(m_DictionaryOfPostsByComments.ContainsKey(i_NumOfComments))
            {
                return m_DictionaryOfPostsByComments[i_NumOfComments];
            }
            else
            {
                return null;
            }
        }

        public ICollection<Photo> GetPhotosByAlbumName(string i_AlbumName)
        {
            return m_DictionaryOfPhotosByAlbumName[i_AlbumName];
        }

        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        //--------------------Feature bestTimeForStatus methods-----------------//
        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        public int GetBestTimeForStatus()
        {
            int hourWithMaxLikesPerPost = -1;
            LikesAndPostsCounter[] likesAndPostCounterArray = new LikesAndPostsCounter[24];
            foreach(Post post in m_User.Posts)
            {
                // get post time
                try
                {
                    DateTime time = post.CreatedTime.GetValueOrDefault();
                    int hour = time.Hour;
                    int numOfLikes = post.LikedBy.Count;
                    likesAndPostCounterArray[hour].AddLikesAndIncPosts(numOfLikes);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            double max = -1;
            for(int i = 0; i < 24; i++)
            {
                LikesAndPostsCounter counter = likesAndPostCounterArray[i];
                if (counter.CalcAvgLikesPerPost() > max)
                {
                    hourWithMaxLikesPerPost = i;
                    max = counter.CalcAvgLikesPerPost();
                }
            }

            return hourWithMaxLikesPerPost;
        }
    }
}
