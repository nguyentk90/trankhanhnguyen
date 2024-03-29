﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using TraCuuThuatNgu.Models;
using System.Web.Security;
using System.Security;

namespace TraCuuThuatNgu
{
    public class ChatHub:Hub
    {
        QAModel qaModel = new QAModel();
        MembershipUser currentUser = Membership.GetUser();

        public void Send(string content, string type, int qid)
        {
            if (type.Equals("Q"))
            {                
                // Add question into database
                Question question = new Question();
                question.QContent = content;
                question.UserId = (Guid)currentUser.ProviderUserKey;
                question.DateAdd = DateTime.Now;
                question.DateModify = DateTime.Now;
                question.Reported = 0;

                string date = String.Format("{0:d/M/yyyy HH:mm}", question.DateAdd);

                int result = qaModel.NewQuestion(question);

                qid = question.QuestionId;


                // Display all clients
                Clients.All.newQuestion(currentUser.UserName,content,type,qid,date);
            }
            else
            { 
                // Add answer into database
                Answer answer = new Answer();
                answer.UserId = (Guid)currentUser.ProviderUserKey;
                answer.AContent = content;
                answer.DateAdd = DateTime.Now;
                answer.Reported = 0;
                answer.QuestionId = qid;

                // Update DateModify
                qaModel.UpdateQuestion(qid);
                
                string date = String.Format("{0:d/M/yyyy HH:mm}", answer.DateAdd);

                qaModel.NewAnswer(answer);

                //answer id
                int aid = answer.AnswerId;

                // Display all clients
                Clients.All.newAnswer(currentUser.UserName, content, type, qid, date, aid);
            }
        }

        // Report 
        public void Report(string type, int id)
        {
            if (type.Equals("Q"))
            {
                qaModel.ReportQuestion(id);
                Clients.Group("Admin").getNewReportNotify();
            }
            else
            {
                qaModel.ReportAnswer(id);
                Clients.Group("Admin").getNewReportNotify();
            }
        }

        // Clear Report
        public void ClearReport(string type, int id)
        {
            if (type.Equals("Q"))
            {
                qaModel.ClearReportQuestion(id);              
            }
            else
            {
                qaModel.ClearReportAnswer(id);                
            }        
        }


        // Delete Question Or Answer
        public void DeleteQA(string type, int id)
        {
            if (type.Equals("Q"))
            {
                qaModel.DeleteQuestion(id);
            }
            else
            {
                qaModel.DeleteAnswer(id);
            }    
        }

        // Connect
        public void Connect(string username)
        {
            if (Roles.IsUserInRole(username, "Admin"))
            {
                Groups.Add(Context.ConnectionId, "Admin");
            }
        }
    }
}