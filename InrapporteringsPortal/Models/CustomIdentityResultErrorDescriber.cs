﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Provider;

namespace InrapporteringsPortal.Web.Models
{
    public class CustomIdentityResultErrorDescriber 
    {
        public string LocalizeErrorMessage(string errorMessage)
        {
            //TODO - hantera {username} i inparameter
            if (errorMessage.Length > 39)
            {
                var str1 = errorMessage.Substring(errorMessage.Length - 17, 16);
                if (str1 == "is already taken")
                    return "Användarnamnet är redan upptaget.";
            }

            switch (errorMessage)
            {
                case "An unknown failure has occurred.":
                    return "Ett okänt fel inträffade";
                case "Incorrect password.":
                    return "Felaktigt PINkod";
                case "A user with this login already exists.":
                    return "En användare med detta användarnamn finns redan.";
                case "User name '{userName}' is invalid, can only contain letters or digits.":
                    return "Användarnamn '{userName}' är felaktigt, kan bara innehålla bokstäver eller siffror.";
                case "Email '{email}' is invalid.":
                    return "Epostadressen '{}' är ogiltig.";
                case "User Name '{userName}' is already taken.":
                    return "Användarnamn  '{userName}' är redan upptaget.";
                case "Name '{userName}' is already taken.":
                    return "Användarnamn  '{userName}' är redan upptaget.";
                case "Email '{email}' is already taken.":
                    return "Epostadress '{email}' används redan.";
                case "Role name '{role}' is invalid.":
                    return "Rollnamnet '{role}' är ej giltigt";
                case "Role name '{role}' is already taken.":
                    return "Rollnamnet '{role}' är redan upptaget.";
                case "User already has a password set.":
                    return "Användaren har redan ett PINkod";
                case "User already in role '{role}'.":
                    return "Anändaren har redan rollen '{role}'.";
                case "User is not in role '{role}'.":
                    return "Användaren har ej rollen '{role}'.";
                case "Passwords must be at least {length} characters.":
                    return "PINkoden måste var minst {length} tecken långt.";
                case "Passwords must have at least one non alphanumeric character.":
                    return "PINkoden måste ha minst ett icke alfanumersikt tecken.";
                case "Passwords must have at least one non letter or digit character.":
                    return "PINkoden måste ha minst ett tecken som inte är alfanumersikt eller en siffra.";
                case "Passwords must have at least one digit ('0'-'9').":
                    return "PINkoden måste ha minst en siffra ('0'-'9').";
                case "Passwords must have at least one lowercase ('a'-'z').":
                    return "PINkoden måste ha minst en liten bokstav ('a'-'z').";
                case "Passwords must have at least one uppercase ('A'-'Z').":
                    return "PINkoden måste ha minst en stor bokstav ('A'-'Z').";
                default:
                    return "";
            }
        }

    }
}