using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DAL.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<MyEvernoteDbContext>
    {
        protected override void Seed(MyEvernoteDbContext context)
        {
            //Admin
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Eren",
                Surname = "Yıldırım",
                Email = "yldrm_eren@hotmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "erenn_yldrm",
                Password = "12345",
                ProfileImgFileName = "user_boy.png",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedBy = "erenn_yldrm"
            };

            //Standart User
            EvernoteUser user = new EvernoteUser()
            {
                Name = "Ahmet",
                Surname = "Doğan",
                Email = "erenyldrm89@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "ahmetdgn",
                Password = "12345",
                ProfileImgFileName = "user_boy.png",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedBy = "ahmetdgn"
            };

            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(user);

            #region |       Fake Data - Users       |

            for (int i = 0; i < 8; i++)
            {
                EvernoteUser fakeUser = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    Password = "12345",
                    ProfileImgFileName = "user_boy.png",
                    CreatedOn = DateTime.Now.AddHours(1),
                    ModifiedOn = DateTime.Now.AddMinutes(65),
                    ModifiedBy = $"user{i}"
                };

                context.EvernoteUsers.Add(fakeUser);
            }

            #endregion

            context.SaveChanges();

            List<EvernoteUser> userList = context.EvernoteUsers.ToList();

            //Adding Fake - Categories
            for (int i = 0; i < 10; i++)
            {
                Category category = new Category()
                {
                    Title = FakeData.TextData.GetAlphabetical(4),
                    Description = FakeData.TextData.GetSentence(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedBy = "erenn_yldrm"
                };

                context.Categories.Add(category);

                //Adding Fake Data - Notes
                for (int k = 0; k < FakeData.NumberData.GetNumber(3, 5); k++)
                {
                    EvernoteUser noteOwner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];

                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(1),
                        Text = FakeData.TextData.GetSentence(),
                        IsDraft = false,
                        //Category = category,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = noteOwner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedBy = noteOwner.Username
                    };

                    category.Notes.Add(note);

                    //Adding Fake Data - Comments
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3, 5); j++)
                    {
                        EvernoteUser commentOwner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];

                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            //Note = note,
                            Owner = commentOwner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedBy = commentOwner.Username
                        };

                        note.Comments.Add(comment);
                    }

                    //Adding Fake Data - Likeds
                    for (int m = 0; m < note.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userList[m]
                        };

                        note.Likeds.Add(liked);
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
