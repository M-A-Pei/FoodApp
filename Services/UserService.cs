using FoodApp.Data;
using FoodApp.Models;

namespace FoodApp.Services;
public class UserService
{
    private readonly ApplicationDbContext _context;
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<UserModel> FindUsers()
    {
        List<UserModel> res = _context.Users.ToList();
        return res;
    }

    public UserModel? GetUser(int id)
    {
        UserModel? user = _context.Users.Find(id);
        return user;
    }

    public UserModel? GetUserByEmail(string email){
        UserModel? res = _context.Users.SingleOrDefault(u => u.email == email);
        return res;
    }
}