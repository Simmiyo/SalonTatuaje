using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SalonTatuaje.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("SalonTatuajeConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new Initp());
        }

        public DbSet<Contact> Contacte { get; set; }
        public DbSet<Artist> Artisti { get; set; }
        public DbSet<Imagine> Imagini { get; set; }
        public DbSet<Programare> Programari { get; set; }
        public DbSet<TipTatuaj> TipuriTatuaje { get; set; }

    public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public class Initp : DropCreateDatabaseAlways<ApplicationDbContext> // DropCreateDatabaseIfModelChanges<ApplicationDbContext> //    
        {
            protected override void Seed(ApplicationDbContext context)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // In Startup iam creating first Admin Role and creating a default Admin User     
                if (!roleManager.RoleExists("Admin"))
                {
                    // first we create Admin rool    
                    var role = new IdentityRole
                    {
                        Name = "Admin"
                    };
                    roleManager.Create(role);

                    //Here we create a Admin super user who will maintain the website                   

                    var user = new ApplicationUser();
                    user.UserName = "simona";
                    user.Email = "simonaAdmin@gmail.com";

                    string userPWD = "BeBe$27";

                    var chkUser = UserManager.Create(user, userPWD);

                    //Add default User to Role Admin    
                    if (chkUser.Succeeded)
                    {
                        var result1 = UserManager.AddToRole(user.Id, "Admin");
                    }
                }

                List<string> editors = new List<string>();
                // creating Creating Manager role     
                if (!roleManager.RoleExists("Editor"))
                {
                    var role = new IdentityRole
                    {
                        Name = "Editor"
                    };
                    roleManager.Create(role);

                    var edit1 = new ApplicationUser();
                    edit1.UserName = "Penelopa";
                    edit1.Email = "penetattoo@yahoo.ro";
                    string edit1pwd = edit1.UserName + "$Sirena_" + edit1.UserName.Length;
                    var chkEdit1 = UserManager.Create(edit1, edit1pwd);
                    if (chkEdit1.Succeeded)
                    {
                        var result1 = UserManager.AddToRole(edit1.Id, role.Name);
                        editors.Add(edit1.Id);
                    }

                    var edit2 = new ApplicationUser();
                    edit2.UserName = "Deborah";
                    edit2.Email = "deborahtattooartist@yahoo.com";
                    string edit2pwd = edit2.UserName + "$Sirena_" + edit2.UserName.Length;
                    var chkEdit2 = UserManager.Create(edit2, edit2pwd);
                    if (chkEdit2.Succeeded)
                    {
                        var result2 = UserManager.AddToRole(edit2.Id, role.Name);
                        editors.Add(edit2.Id);
                    }

                    var edit3 = new ApplicationUser();
                    edit3.UserName = "VictorSilvestru";
                    edit3.Email = "victoraspictoras@unibuc.ro";
                    string edit3pwd = edit3.UserName + "$Sirena_" + edit3.UserName.Length;
                    var chkEdit3 = UserManager.Create(edit3, edit3pwd);
                    if (chkEdit3.Succeeded)
                    {
                        var result3 = UserManager.AddToRole(edit3.Id, role.Name);
                        editors.Add(edit3.Id);
                    }

                    var edit4 = new ApplicationUser();
                    edit4.UserName = "Hector";
                    edit4.Email = "tattoosbyhector@yahoo.ro";
                    string edit4pwd = edit4.UserName + "$Sirena_" + edit4.UserName.Length;
                    var chkEdit4 = UserManager.Create(edit4, edit4pwd);
                    if (chkEdit4.Succeeded)
                    {
                        var result4 = UserManager.AddToRole(edit4.Id, role.Name);
                        editors.Add(edit4.Id);
                    }

                    var edit5 = new ApplicationUser();
                    edit5.UserName = "AugustusRoman";
                    edit5.Email = "gusttatuaje@gmail.com";
                    string edit5pwd = edit5.UserName + "$Sirena_" + edit5.UserName.Length; //AugustusRoman$Sirena_13
                    var chkEdit5 = UserManager.Create(edit5, edit5pwd);
                    if (chkEdit5.Succeeded)
                    {
                        var result5 = UserManager.AddToRole(edit5.Id, role.Name);
                        editors.Add(edit5.Id);
                    }

                    var edit6 = new ApplicationUser();
                    edit6.UserName = "Matraguna";
                    edit6.Email = "matratoo@yahoo.com";
                    string edit6pwd = edit6.UserName + "$Sirena_" + edit6.UserName.Length;
                    var chkEdit6 = UserManager.Create(edit6, edit6pwd);
                    if (chkEdit6.Succeeded)
                    {
                        var result6 = UserManager.AddToRole(edit6.Id, role.Name);
                        editors.Add(edit6.Id);
                    }

                    var edit7 = new ApplicationUser();
                    edit7.UserName = "Persephone";
                    edit7.Email = "artistprs_phtattoo@gmail.com";
                    string edit7pwd = edit7.UserName + "$Sirena_" + edit7.UserName.Length;
                    var chkEdit7 = UserManager.Create(edit7, edit7pwd);
                    if (chkEdit7.Succeeded)
                    {
                        var result7 = UserManager.AddToRole(edit7.Id, role.Name);
                        editors.Add(edit7.Id);
                    }
                }

                // creating Creating Employee role     
                if (!roleManager.RoleExists("Client"))
                {
                    var role = new IdentityRole
                    {
                        Name = "Client"
                    };
                    roleManager.Create(role);

                    var user1 = new ApplicationUser();
                    user1.UserName = "FrantaItalia";
                    user1.Email = "estonia_irlanda@cfr.ro";
                    string user1pwd = "Gaga67*";
                    var chkUser1 = UserManager.Create(user1, user1pwd);  
                    if (chkUser1.Succeeded)
                    {
                        var result1 = UserManager.AddToRole(user1.Id, role.Name);
                    }

                    var user2 = new ApplicationUser();
                    user2.UserName = "Narnia";
                    user2.Email = "narnia118@destiny.com";
                    string user2pwd = "ClhT^53";
                    var chkUser2 = UserManager.Create(user2, user2pwd);  
                    if (chkUser2.Succeeded)
                    {
                        var result2 = UserManager.AddToRole(user2.Id, role.Name);
                    }

                    var user3 = new ApplicationUser();
                    user3.UserName = "Molly";
                    user3.Email = "molly_mutsurutsu@cats.com";
                    string user3pwd = "Molica1!Mica";
                    var chkUser3 = UserManager.Create(user3, user3pwd);
                    if (chkUser3.Succeeded)
                    {
                        var result3 = UserManager.AddToRole(user3.Id, role.Name);
                    }
                }

                context = VerifySaveChanges(context);

                context.Artisti.Add(new Artist
                {
                    Nume = "Penelopa",
                    PozaArtist = new Imagine { PathImagine = "~/Content/Imagini/penelopa/penelopa.jpg" },
                    InfoContact = new Contact
                    {
                        NrTel = "0712345678",
                        Email = "penetattoo@yahoo.ro",
                        Instagram = "@pene.low00pa",
                    },
                    Bio = "Află ce anume iubești și tatuează-ți-l pe piele, căci în suflet este deja tatuat.",
                    IstoricTatuaje = new List<Imagine> {
                    new Imagine{PathImagine = "~/Content/Imagini/penelopa/Tatuaje/penelopa1.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/penelopa/Tatuaje/penelopa2.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/penelopa/Tatuaje/penelopa3.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/penelopa/Tatuaje/penelopa4.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/penelopa/Tatuaje/penelopa5.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/penelopa/Tatuaje/penelopa6.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/penelopa/Tatuaje/penelopa7.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/penelopa/Tatuaje/penelopa8.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/penelopa/Tatuaje/penelopa9.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/penelopa/Tatuaje/penelopa10.jpg"}
                },
                    PortofoliuDesene = new List<Imagine> {
                    new Imagine {PathImagine = "~/Content/Imagini/penelopa/Desene/penelopa_p0.jpg"},
                    new Imagine {PathImagine = "~/Content/Imagini/penelopa/Desene/penelopa_p1.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/penelopa/Desene/penelopa_p2.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/penelopa/Desene/penelopa_p3.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/penelopa/Desene/penelopa_p4.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/penelopa/Desene/penelopa_p5.jpg"}
                },
                    EditorId = editors[0],
                    Editor = context.Users.Find(editors[0])
                });

                context.Artisti.Add(new Artist
                {
                    Nume = "Deborah",
                    PozaArtist = new Imagine { PathImagine = "~/Content/Imagini/deborah/deborah.jpg" },
                    InfoContact = new Contact
                    {
                        NrTel = "0711291180",
                        Email = "deborahtattooartist@yahoo.com",
                        Instagram = "@__deborah_tattoo_artist__"
                    },
                    Bio = "Mama coase, eu tatuez. Fiecare cu acele ei.",
                    IstoricTatuaje = new List<Imagine> {
                    new Imagine{PathImagine = "~/Content/Imagini/deborah/Tatuaje/deborah1.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/deborah/Tatuaje/deborah2.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/deborah/Tatuaje/deborah3.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/deborah/Tatuaje/deborah4.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/deborah/Tatuaje/deborah5.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/deborah/Tatuaje/deborah6.jpg"}
                },
                    PortofoliuDesene = new List<Imagine> {
                    new Imagine {PathImagine ="~/Content/Imagini/deborah/Desene/deborah_p0.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/deborah/Desene/deborah_p1.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/deborah/Desene/deborah_p2.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/deborah/Desene/deborah_p3.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/deborah/Desene/deborah_p4.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/deborah/Desene/deborah_p5.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/deborah/Desene/deborah_p6.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/deborah/Desene/deborah_p7.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/deborah/Desene/deborah_p8.jpg"}
                },
                    EditorId = editors[1],
                    Editor = context.Users.Find(editors[1])
                });

                context.Artisti.Add(new Artist
                {
                    Nume = "Victor-Silvestru",
                    PozaArtist = new Imagine { PathImagine = "~/Content/Imagini/victor/victor.jpg" },
                    InfoContact = new Contact
                    {
                        NrTel = "0788888888",
                        Email = "victoraspictoras@unibuc.ro",
                        Instagram = "@victorsilvar.t",
                    },
                    Bio = "Ador să cunosc oameni noi, să le aflu ideile revigorante și să îi ajut să le pună în practică. Nu există nimic nepotrivit pentru a deveni un tatuaj, pe mine excentricitatea mă încântă.",
                    IstoricTatuaje = new List<Imagine> {
                    new Imagine { PathImagine = "~/Content/Imagini/victor/Tatuaje/victor1.jpg" },
                    new Imagine{PathImagine = "~/Content/Imagini/victor/Tatuaje/victor2.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/victor/Tatuaje/victor3.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/victor/Tatuaje/victor4.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/victor/Tatuaje/victor5.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/victor/Tatuaje/victor6.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/victor/Tatuaje/victor7.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/victor/Tatuaje/victor8.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/victor/Tatuaje/victor9.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/victor/Tatuaje/victor10.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/victor/Tatuaje/victor11.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/victor/Tatuaje/victor12.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/victor/Tatuaje/victor13.jpg"}
                },
                    PortofoliuDesene = new List<Imagine> {
                    new Imagine {PathImagine ="~/Content/Imagini/victor/Desene/victor_p0.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/victor/Desene/victor_p1.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/victor/Desene/victor_p2.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/victor/Desene/victor_p3.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/victor/Desene/victor_p4.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/victor/Desene/victor_p5.jpg"}
                },
                    EditorId = editors[2],
                    Editor = context.Users.Find(editors[2])
                });

                context.Artisti.Add(new Artist
                {
                    Nume = "Hector",
                    PozaArtist = new Imagine { PathImagine = "~/Content/Imagini/hector/hector.jpg" },
                    InfoContact = new Contact
                    {
                        NrTel = "0722779900",
                        Email = "tattoosbyhector@yahoo.ro",
                        Instagram = "@t1a2t3t4o5o6s7.by.hect0r"
                    },
                    Bio = "Absolvent al Facultății de Arte Plastice din București și actual student la masterul de Arte și Design. Înarmat cu inspirație, pregătire profesională și aprigă pasiune, aștept cu nerăbdare să colaborez cu oricine este interesat de proiectele mele!",
                    IstoricTatuaje = new List<Imagine> {
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector1.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector2.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector3.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector4.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector5.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector6.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector7.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector8.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector9.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector10.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector11.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector12.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector13.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector14.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/hector/Tatuaje/hector15.jpg"}
                },
                    PortofoliuDesene = new List<Imagine> {
                    new Imagine {PathImagine ="~/Content/Imagini/hector/Desene/hector_p0.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/hector/Desene/hector_p1.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/hector/Desene/hector_p2.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/hector/Desene/hector_p3.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/hector/Desene/hector_p4.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/hector/Desene/hector_p5.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/hector/Desene/hector_p6.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/hector/Desene/hector_p7.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/hector/Desene/hector_p8.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/hector/Desene/hector_p9.jpg"}
                },
                    EditorId = editors[3],
                    Editor = context.Users.Find(editors[3])
                });

                context.Artisti.Add(new Artist
                {
                    Nume = "Augustus Roman",
                    PozaArtist = new Imagine { PathImagine = "~/Content/Imagini/augustus/augustus.jpg" },
                    InfoContact = new Contact
                    {
                        NrTel = "0776543218",
                        Email = "gusttatuaje@gmail.com",
                    },
                    Bio = "Nu-i lăsa pe alții să-ți pună etichete, fii conștient de cine ești și arată-le tu lor. Începe cu un tatuaj! Aș fi mândru să ți-l fac chiar eu dacă îmi permiți să pătrund puțin în lumea ta.",
                    IstoricTatuaje = new List<Imagine> {
                    new Imagine{PathImagine = "~/Content/Imagini/augustus/Tatuaje/augustus1.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/augustus/Tatuaje/augustus2.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/augustus/Tatuaje/augustus3.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/augustus/Tatuaje/augustus4.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/augustus/Tatuaje/augustus5.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/augustus/Tatuaje/augustus6.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/augustus/Tatuaje/augustus7.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/augustus/Tatuaje/augustus8.jpg"}
                },
                    PortofoliuDesene = new List<Imagine> {
                    new Imagine {PathImagine ="~/Content/Imagini/augustus/Desene/augustus_p0.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/augustus/Desene/augustus_p1.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/augustus/Desene/augustus_p2.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/augustus/Desene/augustus_p3.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/augustus/Desene/augustus_p4.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/augustus/Desene/augustus_p5.jpg"}
                },
                    EditorId = editors[4],
                    Editor = context.Users.Find(editors[4])
                });

                context.Artisti.Add(new Artist
                {
                    Nume = "Mătrăgună",
                    PozaArtist = new Imagine { PathImagine = "~/Content/Imagini/matraguna/matraguna.jpg" },
                    InfoContact = new Contact
                    {
                        NrTel = "0700070007",
                        Email = "matratoo@yahoo.com",
                        Instagram = "@M4TR4GOON4"
                    },
                    Bio = "Viața e prea scurtă ca să te obișnuiești cu monotonia. Umple-te de experiențe și păstrează-le pe tine cu niște tatuaje! Apelează la mine și îmi voi lua angajamentul de a te sfătui cum poți imortaliza într-un mod cât mai estetic ceea ce trăiești și consideri că te reprezintă. Alege o amintire, iar eu mă voi asigura că o păstrezi pe corp în toată splendoarea ei. Nu-ți voi permite să regreți! Te aștept la programare!",
                    IstoricTatuaje = new List<Imagine> {
                    new Imagine{PathImagine = "~/Content/Imagini/matraguna/Tatuaje/matraguna1.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/matraguna/Tatuaje/matraguna2.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/matraguna/Tatuaje/matraguna3.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/matraguna/Tatuaje/matraguna4.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/matraguna/Tatuaje/matraguna5.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/matraguna/Tatuaje/matraguna6.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/matraguna/Tatuaje/matraguna7.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/matraguna/Tatuaje/matraguna8.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/matraguna/Tatuaje/matraguna9.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/matraguna/Tatuaje/matraguna10.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/matraguna/Tatuaje/matraguna11.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/matraguna/Tatuaje/matraguna12.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/matraguna/Tatuaje/matraguna13.jpg"}
                },
                    PortofoliuDesene = new List<Imagine> {
                    new Imagine {PathImagine ="~/Content/Imagini/matraguna/Desene/matraguna_p0.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/matraguna/Desene/matraguna_p1.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/matraguna/Desene/matraguna_p2.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/matraguna/Desene/matraguna_p3.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/matraguna/Desene/matraguna_p4.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/matraguna/Desene/matraguna_p5.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/matraguna/Desene/matraguna_p6.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/matraguna/Desene/matraguna_p7.jpg"}
                },
                    EditorId = editors[5],
                    Editor = context.Users.Find(editors[5])
                });

                context.Artisti.Add(new Artist
                {
                    Nume = "Persephone",
                    PozaArtist = new Imagine { PathImagine = "~/Content/Imagini/persephone/persephone.jpg" },
                    InfoContact = new Contact
                    {
                        NrTel = "0712345678",
                        Email = "artistprs_phtattoo@gmail.com",
                    },
                    Bio = "Câteva frânturi despre mine: Provin dintr-o familie de artiști care timp de secole și-a dedicat existența picturilor și sculprurilor. Încă de la o vârstă fragedă, de când am pus prima oară mâna pe o pensulă, m-am îndrăgostit iremediabil de felul în care mă pot exprima prin artă și nu-mi doresc nimic mai mult decât să îmi cultiv talentul și să le ofer tuturor o parte din mine. Mă aflu aici pentru a transporta frumosul de pe hârtie sau pânză în această nouă arie a picturii în piele și trăiesc prin culorile și formele pe care le las în urmă. Nu mă imaginez făcând altceva.",
                    IstoricTatuaje = new List<Imagine> {
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone1.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone2.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone3.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone4.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone5.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone6.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone7.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone8.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone9.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone10.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone11.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone12.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone13.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone14.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone15.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone16.jpg"},
                    new Imagine{PathImagine = "~/Content/Imagini/persephone/Tatuaje/persephone17.jpg"}
                },
                    PortofoliuDesene = new List<Imagine> {
                    new Imagine {PathImagine ="~/Content/Imagini/persephone/Desene/persephone_p0.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/persephone/Desene/persephone_p1.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/persephone/Desene/persephone_p2.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/persephone/Desene/persephone_p3.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/persephone/Desene/persephone_p4.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/persephone/Desene/persephone_p5.jpg"},
                    new Imagine {PathImagine ="~/Content/Imagini/persephone/Desene/persephone_p6.jpg"},
                },
                    EditorId = editors[6],
                    Editor = context.Users.Find(editors[6])
                });

                context = VerifySaveChanges(context);

                List<ApplicationUser> useri = context.Users.ToList();

                context.TipuriTatuaje.Add(new TipTatuaj
                {
                    Localizare = "spate",
                    Latime = 80,
                    Lungime = 100,
                    Culoare = "color",
                    NumeTip = "spate-80-100-color",
                    Pret = 3500
                });

                context.TipuriTatuaje.Add(new TipTatuaj
                {
                    Localizare = "umar",
                    Latime = 48,
                    Lungime = 22,
                    Culoare = "alb-negru",
                    NumeTip = "umar-48-22-alb-negru",
                    Pret = 400
                });

                context.TipuriTatuaje.Add(new TipTatuaj
                {
                    Localizare = "gamba",
                    Latime = 60,
                    Lungime = 100,
                    Culoare = "alb-negru",
                    NumeTip = "gamba-60-100-alb-negru",
                    Pret = 2000
                });

                context.Programari.Add(new Programare
                {
                    Data = new DateTime(2021, 1, 8, 0, 0, 0),
                    Ora = "17:30",
                    Sala = 1,
                    PretTotal = 1500,
                    TipuriTatuaje = new List<TipTatuaj> {
                    new TipTatuaj { Localizare = "antebrat", Latime = 20, Lungime = 40, Culoare = "alb-negru", NumeTip = "antebrat-20-40-alb-negru", Pret = 1000},
                    new TipTatuaj { Localizare = "picior", Latime = 10, Lungime = 20, Culoare = "alb-negru", NumeTip = "picior-10-20-alb-negru", Pret = 500}
                },
                    Tartist = context.Artisti.Find(5),
                    ArtistId = context.Artisti.Find(5).ArtistCod,
                    Proiecte = new List<Imagine>
                {
                    context.Artisti.Find(5).PortofoliuDesene[0],
                    context.Artisti.Find(5).PortofoliuDesene[1]
                },
                    Tclient = useri.Find(u => u.UserName == "Narnia"),
                    UserId = useri.Find(u => u.UserName == "Narnia").Id
                });

                context.Programari.Add(new Programare
                {
                    Data = new DateTime(2021, 1, 21, 8, 0, 0),
                    Ora = "16:30",
                    Sala = 3,
                    PretTotal = 3000,
                    TipuriTatuaje = new List<TipTatuaj> {
                    new TipTatuaj { Localizare = "abdomen", Latime = 40, Lungime = 50, Culoare = "color", NumeTip = "abdomen-40-50-color", Pret = 2000},
                    new TipTatuaj { Localizare = "piept", Latime = 15, Lungime = 30, Culoare = "color", NumeTip = "piept-15-30-color", Pret = 700},
                    new TipTatuaj { Localizare = "gat", Latime = 10, Lungime = 10, Culoare = "color", NumeTip = "gat-10-10-color", Pret = 300}
                },
                    Tartist = context.Artisti.Find(7),
                    ArtistId = context.Artisti.Find(7).ArtistCod,
                    Proiecte = new List<Imagine>
                {
                    context.Artisti.Find(7).PortofoliuDesene[4],
                    context.Artisti.Find(7).PortofoliuDesene[2],
                    context.Artisti.Find(7).PortofoliuDesene[3],
                },
                    Tclient = useri.Find(u => u.UserName == "FrantaItalia"),
                    UserId = useri.Find(u => u.UserName == "FrantaItalia").Id
                });

                context.Programari.Add(new Programare
                {
                    Data = new DateTime(2021, 2, 2, 14, 40, 0),
                    Ora = "14:00",
                    Sala = 2,
                    PretTotal = 800,
                    TipuriTatuaje = new List<TipTatuaj> {
                    new TipTatuaj { Localizare = "deget", Latime = 5, Lungime = 10, Culoare = "color", NumeTip = "deget-5-10-color", Pret = 100},
                    new TipTatuaj { Localizare = "cap", Latime = 28, Lungime = 26, Culoare = "color", NumeTip = "cap-28-26-color",Pret = 700}
                },
                    Tartist = context.Artisti.Find(2),
                    ArtistId = context.Artisti.Find(2).ArtistCod,
                    Proiecte = new List<Imagine>
                {
                    context.Artisti.Find(2).PortofoliuDesene[1],
                    context.Artisti.Find(2).PortofoliuDesene[4]
                },
                    Tclient = useri.Find(u => u.UserName == "Molly"),
                    UserId = useri.Find(u => u.UserName == "Molly").Id
                });

                context = VerifySaveChanges(context);
                base.Seed(context);
            }

            private ApplicationDbContext VerifySaveChanges(ApplicationDbContext context)
            {
                try
                {
                    context.SaveChanges();
                    return context;
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Console.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                    return null;
                }
            }
        }
    }
}