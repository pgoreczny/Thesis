namespace Thesis.Areas.Identity.Constants
{
    public class Claims
    {
        public class Basic
        {
            public const string IsRegistered = "Is registered";
        }
        public class Users
        {
            public const string UserList = "Show user list";
            public const string UserAdd = "Add user";
            public const string UserEdit = "Edit user";
            public const string UserDelete = "Delete user";
        }

        public class ManageCourses
        {
            public const string CourseList = "Show course list";
            public const string CourseAdd = "Add course";
            public const string CourseEdit = "Edit course";
            public const string CourseDelete = "Delete course";
            public const string ManageUsers = "Manage users";
        }

        public class UserCourses
        {
            public const string SeeCourses = "See courses";
            public const string JoinCourse = "Join course";
            public const string AccessCourse = "Access course";
            public const string ParticipateInCourse = "Participate in course";
        }

        public class Roles
        {
            public const string AssignRole = "Assign role to user";
        }

        public class Forum
        {
            public const string ReadPost = "Read a post";
            public const string WritePost = "Write a post";
            public const string EditYour = "Edit your post or comment";
            public const string CommentPost = "Comment post";
            public const string EditAny = "Edit any post or comment";
            public const string Delete = "Delete posts and comments";
        }
    }
}
