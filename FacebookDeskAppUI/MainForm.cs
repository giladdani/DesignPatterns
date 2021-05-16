using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;
using FacebookDeskAppLogic;
using Message = FacebookWrapper.ObjectModel.Message;

namespace FacebookDeskAppUI
{
    public partial class MainForm : Form
    {
        // Private Members
        private const string k_AllTitle = "All";
        private const string k_PlacesTitle = "Places";
        private const string k_CommentsTitle = "Comments";
        private const string k_LikesTitle = "Likes";
        private LoggedinUserData m_LoggedInUserData;
        private static readonly object sr_lock = new object();

        // Ctor
        public MainForm()
        {
            InitializeComponent();
            m_LoggedInUserData = Singleton<LoggedinUserData>.Instance;
            InitFormDetails();
        }

        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        //------------------------  General Methods  -------------------------//
        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        private void setListBox<T>(ICollection<T> i_List, ListBox i_ListBox)
        {
            lock(sr_lock)
            {
                i_ListBox.Items.Clear();
                foreach(T elem in i_List)
                {
                    i_ListBox.Items.Add(elem);
                }
            }
        }

        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        //------------------- Setting ListBox of posts Methods----------------//
        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        private void setPostsListByPlaces(string i_PlaceName)
        {
            ICollection<Post> listOfPosts = m_LoggedInUserData.GetPostsByPlaceName(i_PlaceName);
            if(listOfPosts != null)
            {
                ICollection<PostWrapper> listOfPostsWrapper = generateListOfPostsWrappers(listOfPosts);
                setListBox(listOfPostsWrapper, listBoxPosts);
            }
        }

        private void setListBoxPostsByComments(string i_NumOfComments)
        {
            ICollection<Post> listOfPosts = m_LoggedInUserData.GetPostsByNumOfComments(i_NumOfComments);
            if(listOfPosts != null)
            {
                ICollection<PostWrapper> listOfPostsWrapper = generateListOfPostsWrappers(listOfPosts);
                setListBox(listOfPostsWrapper, listBoxPosts);
            }
        }

        private void setListBoxPostsByLikes(string i_NumOfLikes)
        {
            ICollection<Post> listOfPosts = m_LoggedInUserData.GetPostsByNumOfLikes(i_NumOfLikes);
            if(listOfPosts != null)
            {
                ICollection<PostWrapper> listOfPostsWrapper = generateListOfPostsWrappers(listOfPosts);
                setListBox(listOfPostsWrapper, listBoxPosts);
            }
        }

        private void setListBoxPostsByListOfAll()
        {
            listBoxPosts.Items.Clear();
            ICollection<Post> listOfPosts = m_LoggedInUserData.FetchAllPosts();
            if(listOfPosts != null)
            {
                ICollection<PostWrapper> listOfPostsWrapper = generateListOfPostsWrappers(listOfPosts);
                setListBox(listOfPostsWrapper, listBoxPosts);
            }
        }

        private void listBoxPosts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PostWrapper postWrapper = listBoxPosts.SelectedItem as PostWrapper;
                postBindingSource.DataSource = postWrapper.Post;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to show post");
            }
        }

        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        //----------- Setting combobox of sub filter of posts Methods---------//
        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        private void setComboboxPostsSubFilter(ICollection<string> i_Options)
        {
            foreach (string option in i_Options)
            {
                comboBoxPostsSubFilter.Items.Add(option);
            }
        }

        private void setComboboxPostsSubFilterByPlaces()
        {
            labelPostsSubFilter.Text = "Filter by places";
            m_LoggedInUserData.FetchPostsByPlaces();
            List<string> listOfPlacesNames = m_LoggedInUserData.GetPlaceNamesOfPosts();
            setComboboxPostsSubFilter(listOfPlacesNames);
        }

        private void setComboboxPostsSubFilterByLikes()
        {
            try
            {
                m_LoggedInUserData.FetchPostsByNumOfLikes();
                labelPostsSubFilter.Text = "Filter by likes";
                setComboboxPostsSubFilterByNumericOptions();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Could not fetch posts :(");
            }
        }

        private void setComboboxPostsSubFilterByComments()
        {
            labelPostsSubFilter.Text = "Filter by comments";
            m_LoggedInUserData.FetchPostsByNumOfComments();
            setComboboxPostsSubFilterByNumericOptions();
        }

        private void setComboboxPostsSubFilterByNumericOptions()
        {
            List<string> listOfNumOfOptions = new List<string>();
            listOfNumOfOptions.Add("1-10");
            listOfNumOfOptions.Add("11-20");
            listOfNumOfOptions.Add("20-50");
            listOfNumOfOptions.Add("51-100");
            listOfNumOfOptions.Add("100-200");
            listOfNumOfOptions.Add("Above 200");
            setComboboxPostsSubFilter(listOfNumOfOptions);
        }

        private void comboBoxPostsSubFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string optionOfFilter = comboBoxPostsFilter.Text;
            string optionOfSubFilter = comboBoxPostsSubFilter.Text;

            if (optionOfFilter == k_PlacesTitle)
            {
                setPostsListByPlaces(optionOfSubFilter);
            }
            else if (optionOfFilter == k_LikesTitle)
            {
                setListBoxPostsByLikes(optionOfSubFilter);
            }
            else if (optionOfFilter == k_CommentsTitle)
            {
                setListBoxPostsByComments(optionOfSubFilter);
            }
        }

        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        //-----------------combobox of filter of posts Methods----------------//
        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        private void setComboBoxPostsFilter()
        {
            comboBoxPostsSubFilter.Visible = false;
            labelPostsSubFilter.Visible = false;
            comboBoxPostsFilter.Items.Clear();
            comboBoxPostsFilter.Items.Add(k_AllTitle);
            comboBoxPostsFilter.Items.Add(k_PlacesTitle);
            comboBoxPostsFilter.Items.Add(k_LikesTitle);
            comboBoxPostsFilter.Items.Add(k_CommentsTitle);
        }

        private void comboBoxPostsFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string category = comboBoxPostsFilter.Text;
            if (category == k_AllTitle)
            {
                comboBoxPostsSubFilter.Visible = false;
                labelPostsSubFilter.Visible = false;
                setListBoxPostsByListOfAll();
            }
            else if (category == k_PlacesTitle)
            {
                setGeneralOptionsToSubFilterComponents();
                setComboboxPostsSubFilterByPlaces();
            }
            else if (category == k_LikesTitle)
            {
                setGeneralOptionsToSubFilterComponents();
                setComboboxPostsSubFilterByLikes();
            }
            else if (category == k_CommentsTitle)
            {
                setGeneralOptionsToSubFilterComponents();
                setComboboxPostsSubFilterByComments();
            }
        }

        private void setGeneralOptionsToSubFilterComponents()
        {
            comboBoxPostsSubFilter.Visible = true;
            labelPostsSubFilter.Visible = true;
            comboBoxPostsSubFilter.Items.Clear();
        }

        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        //---------------------------login Methods----------------------------//
        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        private void listBoxAlbums_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBoxAlbums.SelectedItem != null)
            {
                try
                {
                    listBoxPhotos.Items.Clear();
                    string albumName = (listBoxAlbums.SelectedItem as Album).Name;
                    List<Photo> photos = m_LoggedInUserData.FetchPhotosByAlbumName(albumName);
                    setListBox(photos, listBoxPhotos);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error fetching photos from album");
                }
            }
        }

        private void listBoxPhotos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBoxPhotos.SelectedItem is Photo photo)
            {
                photoBindingSource.DataSource = photo;
            }
        }

        private void listBoxFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            friendsDetailsBindingSource.DataSource = listBoxFriends.SelectedItem as User;
        }

        private void listBoxGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBindingSource.DataSource = listBoxGroups.SelectedItem as Group;
        }

        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        //---------------------------Wrapper Methods--------------------------//
        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        private ICollection<PostWrapper> generateListOfPostsWrappers(ICollection<Post> i_ListOfPosts)
        {
            ICollection<PostWrapper> listOfPostWrapper = new List<PostWrapper>();
            foreach (Post post in i_ListOfPosts)
            {
                PostWrapper postWrapper = new PostWrapper(post);
                listOfPostWrapper.Add(postWrapper);
            }

            return listOfPostWrapper;
        }

        private void buttonBestHourToPost_Click(object sender, EventArgs e)
        {
            try
            {
                int bestHourToPost = m_LoggedInUserData.GetBestTimeForStatus();
                labelBestHourToPostVal.Text = $"Best hour to post: {bestHourToPost}:00";
            }
            catch(Exception ex)
            {
                MessageBox.Show("Could not calculate Best time :(");
            }
        }

        private void buttonCreatePost_Click(object sender, EventArgs e)
        {
            try
            {
                Status postedStatus = m_LoggedInUserData.User.PostStatus(richTextBoxCreatePost.Text);
                MessageBox.Show("Post was published successfully!");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Could not publish post :(");
            }
        }

        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        //----------------------initialization Methods------------------------//
        //----------------------------------------------------------------------//
        //----------------------------------------------------------------------//
        public void InitFormDetails()
        {
            setProfileDetails();
            setAboutPage();
            setPostsPage();
            setAlbumsPage();
            setFriendsPage();
            setGroupsPage();
            //new Thread(setPostsPage).Start();
            //new Thread(setAlbumsPage).Start();
            //new Thread(setFriendsPage).Start();
            //new Thread(setGroupsPage).Start();
        }

        private void setProfileDetails()
        {
            labelProfileName.Text = "Welcome, " + m_LoggedInUserData.User.Name;
            pictureBoxProfilePhoto.ImageLocation = m_LoggedInUserData.User.PictureNormalURL;
        }

        private void setAboutPage()
        {
            labelAboutNameVal.Text = m_LoggedInUserData.User.Name;
            pictureBoxAboutPhoto.ImageLocation = m_LoggedInUserData.User.PictureNormalURL;
            labelAboutGenderVal.Text = m_LoggedInUserData.User.Gender.ToString();
            labelAboutBirthYearVal.Text = m_LoggedInUserData.User.Birthday;
        }

        private void setPostsPage()
        {
            setComboBoxPostsFilter();
        }

        private void setAlbumsPage()
        {
            try
            {
                listBoxAlbums.DisplayMember = "Name";
                listBoxPhotos.DisplayMember = "Id";
                m_LoggedInUserData.FetchAlbums();
                ICollection<Album> albums = m_LoggedInUserData.GetAllAlbums();
                setListBox(albums, listBoxAlbums);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in fetching albums");
            }
        }

        private void setFriendsPage()
        {
            try
            {
                listBoxFriends.DisplayMember = "Name";
                ICollection<User> friends = m_LoggedInUserData.GetAllFriends();
                setListBox(friends, listBoxFriends);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in fetching friends");
            }
        }

        private void setGroupsPage()
        {
            try
            {
                listBoxGroups.DisplayMember = "Name";
                ICollection<Group> groups = m_LoggedInUserData.GetAllGroups();
                setListBox(groups, listBoxGroups);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem in fetching groups");
            }
        }
    }
}
