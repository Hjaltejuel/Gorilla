﻿using Entities.RedditEntities;
using Gorilla.AuthenticationGorillaAPI;
using Gorilla.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace UITEST.ViewModel
{
    public class CreatePostPageViewModel : BaseViewModel
    {
        private readonly IRedditAPIConsumer _consumer;
        private Subreddit CurrentSubreddit;

        public delegate void PostSent();
        public event PostSent SentSuccesfulEvent;
        public event PostSent SentUnsuccesfulEvent;

        public Subreddit currentSubreddit
        {
            get { return CurrentSubreddit; }
            set
            {
                if (value != CurrentSubreddit)
                {
                    CurrentSubreddit = value;
                    OnPropertyChanged("CurrentSubreddit");
                }
            }
        }


        public CreatePostPageViewModel(IAuthenticationHelper helper, INavigationService service, IRedditAPIConsumer consumer) : base(service)
        {
            _consumer = consumer;
            _helper = helper;
        }

        public async Task CreateNewPostAsync(string title, string body = "")
        {
            var response = await _consumer.CreatePostAsync(CurrentSubreddit, title, "self", body);
            if (response.Item1 == HttpStatusCode.OK)
            {
                SentSuccesfulEvent.Invoke();
            }
            else
            {
                SentUnsuccesfulEvent.Invoke();
            }
        }
    }
}