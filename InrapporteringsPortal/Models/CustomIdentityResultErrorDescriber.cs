using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Provider;

namespace InrapporteringsPortal.Models
{
    public class CustomIdentityResultErrorDescriber 
    {
        public string LocalizeErrorMessage(string errorMessage)
        {
            switch (errorMessage)
            {
                case "An unknown failure has occurred.":
                    return "Ett okänt fel inträffade";
                    break;
                case "Incorrect password.":
                    return "Felaktigt lösenord";
                    break;
                case "A user with this login already exists.":
                    return "En användare med detta användarnamn finns redan.";
                    break;
                case "User name '{userName}' is invalid, can only contain letters or digits.":
                    return "Användarnamn '{userName}' är felaktigt, kan bara innehålla bokstäver eller siffror.";
                    break;
                case "Email '{email}' is invalid.":
                    return "Epostadressen '{}' är ogiltig.";
                    break;
                case "User Name '{userName}' is already taken.":
                    return "Användarnamn  '{userName}' är redan upptaget.";
                    break;
                case "Email '{email}' is already taken.":
                    return "Epostadress '{email}' används redan.";
                    break;
                case "Role name '{role}' is invalid.":
                    return "Rollnamnet '{role}' är ej giltigt";
                    break;
                case "Role name '{role}' is already taken.":
                    return "Rollnamnet '{role}' är redan upptaget.";
                    break;
                case "User already has a password set.":
                    return "Användaren har redan ett lösenord";
                    break;
                case "User already in role '{role}'.":
                    return "Anändaren har redan rollen '{role}'.";
                    break;
                case "User is not in role '{role}'.":
                    return "Användaren har ej rollen '{role}'.";
                    break;
                case "Passwords must be at least {length} characters.":
                    return "Lösenordet måste var minst {length} tecken långt.";
                    break;
                case "Passwords must have at least one non alphanumeric character.":
                    return "Lösenordet måste ha minst ett icke alfanumersikt tecken.";
                    break;
                case "Passwords must have at least one non letter or digit character.":
                    return "Lösenordet måste ha minst ett icke alfanumersikt tecken.";
                    break;
                case "Passwords must have at least one digit ('0'-'9').":
                    return "Lösenordet måste ha minst en siffra ('0'-'9').";
                    break;
                case "Passwords must have at least one lowercase ('a'-'z').":
                    return "Lösenordet måste ha minst en liten bokstav ('a'-'z').";
                    break;
                case "Passwords must have at least one uppercase ('A'-'Z').":
                    return "Lösenordet måste ha minst en stor bokstav ('A'-'Z').";
                    break;
                default:
                    return "";
                    break;
            }
        }

    }
}