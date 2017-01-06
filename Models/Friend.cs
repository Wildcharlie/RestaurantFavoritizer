using RestaurantFavoritizer.DAL;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace RestaurantFavoritizer.Models
{
    public class Friend
    {
        public int FriendID { get; set; }
        public int FavUserID { get; set; }
        public int FriendFavUserID { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public static class FriendMaster
    {
        public static string AddFriend(string email, FavContext db = null)
        {
            if (db == null)
            {
                db = new FavContext();
            }

            FavUser user, friendUser;

            friendUser = FavUserMaster.GetFavUserByName(email, db);

            user = FavUserMaster.GetCurrentUser(db);
 
            if (friendUser == null)
            {
                // Send Email inviting to Favoritizer
                var friendMessage = new SendGridMessage();

                friendMessage.From = new MailAddress("DoNotReply@favoritizer.azurewebsites.net");

                friendMessage.AddTo(email);

                friendMessage.Subject = "You have been invited to join Restaurant Favoritizer by a friend!";

                friendMessage.Html = "<p>Someone you know has attempted to add you as a friend on Restaurant Favoritizer. If you would like to check us out, go to <a href=\"https://favoritizer.azurewebsites.net\">Favoritizer</a></p> ";
                friendMessage.Text = "Someone you know has attempted to add you as a friend on Restaurant Favoritizer. If you would like to check us out, go to favoritizer.azurewebsites.net";

                friendMessage.EnableClickTracking(true);
                var credentials = new NetworkCredential(
                       ConfigurationManager.AppSettings["mailAccount"],
                       ConfigurationManager.AppSettings["mailPassword"]
                       );
                var transportWeb = new Web(credentials);
                transportWeb.DeliverAsync(friendMessage);

                return "User Not found. We have emailed them an invitation to favoritizer!";
            }
            else
            {
                Friend f = db.Friends.FirstOrDefault(x => x.FavUserID == user.FavUserID && x.FriendFavUserID == friendUser.FavUserID);

                if (friendUser.FavUserID == user.FavUserID)
                {
                    return "You Cannot Add yourself!";
                }

                if (f != null)
                {
                    return "User is already your friend.";
                }

                // Make new Friend;
                try
                {
                    Friend friend;
                    Perk p;

                    p = new Perk { FavUserID = user.FavUserID, Value = 5, PerkType = PerkType.AddedFriend, CreateDate = DateTime.Now };

                    friend = new Friend { FavUserID = user.FavUserID, FriendFavUserID = friendUser.FavUserID, CreateDate = DateTime.Now };

                    db.Friends.Add(friend);
                    db.Perks.Add(p);
                    db.SaveChanges();
                }
                catch
                {

                }
                return "New friend added!";
            }
        }
    }
}
