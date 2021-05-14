namespace FacebookDeskAppLogic
{
    public class LikesAndPostsCounter
    {
        // Private Members
        private int m_TotalLikes;
        private int m_NumOfPosts;

        // Constructors
        public LikesAndPostsCounter()
        {
            m_TotalLikes = 0;
            m_NumOfPosts = 0;
        }

        // Public Methods
        public double CalcAvgLikesPerPost()
        {
            return (double)m_TotalLikes / m_NumOfPosts;
        }

        public void AddLikesAndIncPosts(int i_NumOfLikes)
        {
            m_TotalLikes += i_NumOfLikes;
            m_NumOfPosts++;
        }

        // Properties
        public int TotalLikes
        {
            get
            {
                return m_TotalLikes;
            }

            set
            {
                m_TotalLikes = value;
            }
        }

        public int NumOfPosts
        {
            get
            {
                return m_NumOfPosts;
            }

            set
            {
                m_NumOfPosts = value;
            }
        }
    }
}
