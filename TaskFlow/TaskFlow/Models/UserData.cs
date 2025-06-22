namespace TaskFlow.Models;

public class UserData : BaseModel
{
    private string _id;
    private string _token;
    private string _email;
    private string _confirmEmail;
    private string _pass;

    public string Id
    {
        get => _id;
        set
        {
            if (_id == value)
            {
                return;
            }

            _id = value;
            OnPropertyChanged();
        }
    }

    public string Token
    {
        get => _token;
        set
        {
            if (_token == value)
            {
                return;
            }

            _token = value;
            OnPropertyChanged();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (_email == value)
            {
                return;
            }

            _email = value;
            OnPropertyChanged();
        }
    }

    public string ConfirmEmail
    {
        get => _confirmEmail;
        set
        {
            if (_confirmEmail == value)
            {
                return;
            }

            _confirmEmail = value;
            OnPropertyChanged();
        }
    }

    public string Pass
    {
        get => _pass;
        set
        {
            if (_pass == value)
            {
                return;
            }

            _pass = value;
            OnPropertyChanged();
        }
    }
}