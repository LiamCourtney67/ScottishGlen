﻿using Microsoft.EntityFrameworkCore;
using ScottishGlenAssetTracking.Data;
using ScottishGlenAssetTracking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace ScottishGlenAssetTracking.Services
{
    /// <summary>
    /// AccountService class used to interact with the database for the Account entity.
    /// </summary>
    public class AccountService
    {
        // Private field for the ScottishGlenContext.
        private readonly ScottishGlenContext _context;

        /// <summary>
        /// Constructor for the AccountService class.
        /// </summary>
        /// <param name="context">ScottishGlenContext to be injected using dependency injection.</param>
        public AccountService(ScottishGlenContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method to add an Account to the database.
        /// </summary>
        /// <param name="account">Account to be added to the database.</param>
        /// <returns>True if added to the database, false if not.</returns>
        public bool AddAccount(Account account)
        {
            // Check if the account already exists
            if (_context.Accounts.Any(a => a.Email == account.Email))
            {
                return false;
            }

            // Add the account to the database and save the changes
            _context.Accounts.Add(account);
            _context.SaveChanges();

            return true;
        }

        /// <summary>
        /// Method to get an Account from the database by its email.
        /// </summary>
        /// <param name="email">Email of the Account to be retrieved.</param>
        /// <returns>Account from the database with the chosen email.</returns>
        public Account GetAccount(string email)
        {
            return _context.Accounts.Include(a => a.Employee).ThenInclude(e => e.Department).FirstOrDefault(a => a.Email == email);
        }

        /// <summary>
        /// Method to get all Accounts from the database.
        /// </summary>
        /// <returns>List of all Accounts from the database.</returns>
        public List<Account> GetAccounts()
        {
            return _context.Accounts.Include(a => a.Employee).ThenInclude(e => e.Department).ToList();
        }

        /// <summary>
        /// Method to get all Accounts from the database for a specific department.
        /// </summary>
        /// <param name="departmentId">Id of Department to get the Accounts for.</param>
        /// <returns>List of Accounts from the database for the chosen Department.</returns>
        public List<Account> GetAccounts(int departmentId)
        {
            return _context.Accounts.Include(a => a.Employee).ThenInclude(e => e.Department).Where(a => a.Employee.Department.Id == departmentId).ToList();
        }

        /// <summary>
        /// Method to get all Accounts from the database for a specific department and admin status.
        /// </summary>
        /// <param name="departmentId">Id of the Department to retrieve all the accounts for.</param>
        /// <param name="isAdmin">If the Account is an admin or not.</param>
        /// <returns>List of Accounts from the database for the chosen Department and admin status.</returns>
        public List<Account> GetAccounts(int departmentId, bool isAdmin)
        {
            return _context.Accounts.Include(a => a.Employee).ThenInclude(e => e.Department).Where(a => a.Employee.Department.Id == departmentId && a.IsAdmin == isAdmin).ToList();
        }

        /// <summary>
        /// Method to update an Account in the database.
        /// </summary>
        /// <param name="account">Account to be updated in the database.</param>
        /// <returns>True if updated in the database, false if not.</returns>
        public bool UpdateAccount(Account account)
        {
            // Check if the account exists
            if (_context.Accounts.Any(a => a.Id == account.Id))
            {
                // Check if the email is already in use by another account
                if (_context.Accounts.Any(a => a.Email == account.Email && a.Id != account.Id))
                {
                    return false;
                }

                // Update the account and save the changes
                _context.Accounts.Update(account);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to delete an Account from the database.
        /// </summary>
        /// <param name="email">Email of the Account to be deleted.</param>
        /// <returns></returns>
        public bool DeleteAccount(string email)
        {
            // Check if the account exists
            var account = _context.Accounts.FirstOrDefault(a => a.Email == email);

            if (account != null)
            {
                // Remove the account from the database and save the changes
                _context.Accounts.Remove(account);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to authenticate an Account using the email and password.
        /// </summary>
        /// <param name="email">Email of the Account to be authenticated.</param>
        /// <param name="password">Password of the Account to be authenticated.</param>
        /// <returns>Authenticated Account if successful, null if not.</returns>
        /// <exception cref="AuthenticationException">Thrown if the email or password is incorrect.</exception>
        public Account AuthenticateAccount(string email, string password)
        {
            // Retrieve account and verify password
            Account account = GetAccount(email);

            if (account != null && account.VerifyPassword(password)) { return account; }
            else { throw new AuthenticationException("Invalid email or password."); }
        }

        /// <summary>
        /// Method to update the password of an Account.
        /// </summary>
        /// <param name="email">Email of the Account for password change.</param>
        /// <param name="password">New password to be changed to.</param>
        /// <returns></returns>
        public bool UpdatePassword(string email, string password)
        {
            // Retrieve account and update password
            var account = _context.Accounts.FirstOrDefault(a => a.Email == email);

            if (account != null)
            {
                account.Password = password;
                _context.Accounts.Update(account);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to set an Account as an administrator.
        /// </summary>
        /// <param name="email">Email of the Account to be set to admin.</param>
        /// <returns>True if set to admin, false if not.</returns>
        public bool SetAccountToAdmin(string email)
        {
            // Retrieve account and update admin status
            var account = _context.Accounts.FirstOrDefault(a => a.Email == email);

            if (account != null)
            {
                account.IsAdmin = true;
                _context.Accounts.Update(account);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to set the Employee entity for an Account.
        /// </summary>
        /// <param name="email">Email of the Account for the Employee to be set.</param>
        /// <param name="employee">Employee for the Account.</param>
        /// <returns>True if Employee set, false if not.</returns>
        public bool SetAccountEmployee(string email, Employee employee)
        {
            // Retrieve account and update employee
            Account account = _context.Accounts.FirstOrDefault(a => a.Email == email);
            Employee existingEmployee = _context.Employees.FirstOrDefault(e => e.Id == employee.Id);

            if (account != null)
            {
                // Check if the employee is in the IT department and remove admin status if not.
                if (existingEmployee.Department.Id != 5)
                {
                    account.IsAdmin = false;
                }

                // Set the new navigational properties.
                account.Employee = existingEmployee;
                existingEmployee.Account = account;

                // Change the email to the account email.
                existingEmployee.Email = account.Email;

                _context.Accounts.Update(account);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Method to check if an Employee has an Account.
        /// </summary>
        /// <param name="employeeId">Employee Id to be checked.</param>
        /// <returns></returns>
        public bool EmployeeHasAccount(int employeeId) => _context.Accounts.Any(a => a.Employee.Id == employeeId);

        /// <summary>
        /// Method to get the Account Id for an Employee.
        /// </summary>
        /// <param name="employeeId">Employee Id to get the Account Id for.</param>
        /// <returns>Id of the Account.</returns>
        public int GetAccountIdForEmployee(int employeeId) => _context.Accounts.FirstOrDefault(a => a.Employee.Id == employeeId).Id;
    }
}
