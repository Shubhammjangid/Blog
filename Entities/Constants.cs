namespace Entities;
public static class Constants
{
    public static class Keys
    {
        public const string ADMIN = "Admin";
        public const string INVALID_DATA = "Invalid data";
        public const string FORBBIDEN = "Forbbiden";
    }
    public static class Users
    {
        public const string USER_ALREADY_EXISTS_WITH_THIS_EMAIL = "User already exists with this email";
        public const string USER_CREATED_SUCCESSFULLY = "User created succesfully";
        public const string ERROR_OCCURED_CREATING_USER = "An error occurred while creating the user";
        public const string INVALID_USER_CRENDENTIAL = "Invalid credential";
        public const string VERIFY_EMAIL = "Please verify your email address";
    }

    public static class Blog
    {
        public const string BLOG_ADDED = "Blog added succesfully";
        public const string BLOG_APPROVED_SUCCESSFULLY = "Blog approved succesfully";
        public const string BLOG_DIS_APPROVED_SUCCESSFULLY = "Blog disapproved succesfully";
        public const string BLOG_DOES_NOT_EXISTS = "Blog does not exists";
        public const string ADD_REASON_FOR_NOT_APPROVAL = "Please add reason for not approval";
        public const string DIS_APPROVED = "Your blog is disapproved by admin";
        public const string BLOG_ACTIVE_STATUS_UPDATED = "Blog active statuc updated succesfully";
    }

    public static class BLOG_COMMENT
    {
        public const string LIMIT_REACHED = "Sorry, you've reached the maximum limit of comments allowed on this blog. You can only comment 5 times per blog post.";
        public const string MAX_LENGTH = "Sorry, your comment exceeds the maximum allowed length of 300 characters. Please shorten your comment.";
        public const string ADDED_SUCCESSFULLY = "Comment added succesfully";
    }
}