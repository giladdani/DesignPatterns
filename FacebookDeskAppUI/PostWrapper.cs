using FacebookWrapper.ObjectModel;

namespace FacebookDeskAppUI
{
    public class PostWrapper
    {
        // Private Members
        private Post m_Post;

        // Constructors
        public PostWrapper(Post i_Post)
        {
            m_Post = i_Post;
        }

        // Public Methods
        public override string ToString()
        {
            if(string.IsNullOrEmpty(m_Post.Name))
            {
                return string.Format("Unnamed Post");
            }
            else
            {
                return m_Post.Name;
            }
        }

        // Properties
        public Post Post
        {
            get
            {
                return m_Post;
            }

            set
            {
                m_Post = value;
            }
        }
    }
}
