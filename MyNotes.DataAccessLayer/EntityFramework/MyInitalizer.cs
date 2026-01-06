using MyNotes.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MyNotes.DataAccessLayer.EntityFramework
{
    public class MyInitalizer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        override protected void Seed(DatabaseContext context)
        {
            // Buraya başlangıç verileri ekleyebilirsiniz.

            EverNoteUser admin = new EverNoteUser()
            {
                Name = "Abbas",
                Surname = "Çoban",
                Email = "abbscbn@gmail.com",
                Password = "1",
                Username = "abbas",
                IsActive = true,
                IsAdmin = true,
                ActiveGuid = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                ModifedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "abbas"

            };

            EverNoteUser standartUser = new EverNoteUser()
            {
                Name = "Emin",
                Surname = "Sezgin",
                Email = "emin@mail.com",
                Password = "1",
                Username = "emin",
                IsActive = true,
                IsAdmin = false,
                ActiveGuid = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                ModifedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "abbas"

            };

            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standartUser);

            // adding fake users

            for (int i = 0; i < 8; i++)
            {
                EverNoteUser user = new EverNoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    Username = $"user{i}",
                    Password = "1",
                    IsActive = true,
                    IsAdmin = false,
                    ActiveGuid = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    ModifedOn = DateTime.Now,
                    ModifiedUsername = "abbas"
                };
                context.EvernoteUsers.Add(user);
            }
            // saving users

            context.SaveChanges();

            //adding fake categories

            for (int i = 0; i < FakeData.NumberData.GetNumber(1, 8); i++)
            {
                Category category = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifedOn = DateTime.Now,
                    ModifiedUsername = "abbas"
                };
                context.Categories.Add(category);

                // adding fake notes

                for (int k = 0; k < FakeData.NumberData.GetNumber(5, 15); k++)
                {
                    EverNoteUser owner = k % 2 == 0 ? admin : standartUser;
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        IsDraft = false,
                        LikeCount = 0,
                        Owner = owner,

                        CreatedOn = DateTime.Now,
                        ModifedOn = DateTime.Now,
                        ModifiedUsername = "abbas"
                    };
                    category.Notes.Add(note);

                    // adding fake comments

                    for (int j = 0; j < FakeData.NumberData.GetNumber(3, 10); j++)
                    {
                        EverNoteUser commentOwner = j % 2 == 0 ? admin : standartUser;
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),

                            Owner = commentOwner,
                            CreatedOn = DateTime.Now,
                            ModifedOn = DateTime.Now,
                            ModifiedUsername = "abbas"
                        };
                        note.Comments.Add(comment);


                    }

                    List<EverNoteUser> userList = context.EvernoteUsers.ToList<EverNoteUser>();

                    // adding fake likes

                    for (int m = 0; m < FakeData.NumberData.GetNumber(1, 5); m++)
                    {

                        Liked like = new Liked()
                        {
                            LikedUser = userList[m]

                        };
                        note.Likes.Add(like);
                        note.LikeCount = note.LikeCount + 1;
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
