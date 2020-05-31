using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VkNet;
using VkNet.Abstractions;
using VkNet.Abstractions.Authorization;
using VkNet.Abstractions.Category;
using VkNet.Abstractions.Core;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;
using VkNet.Utils.AntiCaptcha;
using Xunit;

namespace VkBotHelperTests.TestDouble
{
    class VkApiDummy : IVkApi
    {
        internal List<GroupUpdateType> ExpectedActions = new List<GroupUpdateType>();
        internal List<GroupUpdateType> ActualActions = new List<GroupUpdateType>();

        class MessageCategoryDummy : IMessagesCategory
        {
            private readonly VkApiDummy _dummy;

            public MessageCategoryDummy(VkApiDummy dummy)
            {
                _dummy = dummy;
            }

            public Task<bool> AddChatUserAsync(long chatId, long userId)
            {
                throw new NotImplementedException();
            }

            public Task<bool> AllowMessagesFromGroupAsync(long groupId, string key)
            {
                throw new NotImplementedException();
            }

            public Task<long> CreateChatAsync(IEnumerable<ulong> userIds, string title)
            {
                throw new NotImplementedException();
            }

            public Task<IDictionary<ulong, bool>> DeleteAsync(IEnumerable<ulong> messageIds, bool? spam = null,
                ulong? groupId = null, bool? deleteForAll = null)
            {
                throw new NotImplementedException();
            }

            public Task<Chat> DeleteChatPhotoAsync(ulong chatId, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<bool> DenyMessagesFromGroupAsync(long groupId)
            {
                throw new NotImplementedException();
            }

            public Task<bool> EditChatAsync(long chatId, string title)
            {
                throw new NotImplementedException();
            }

            public Task<VkCollection<Message>> GetByIdAsync(IEnumerable<ulong> messageIds, IEnumerable<string> fields,
                ulong? previewLength = null, bool? extended = null,
                ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<MessageSearchResult> SearchAsync(MessagesSearchParams @params)
            {
                throw new NotImplementedException();
            }

            public Task<long> SendAsync(MessagesSendParams @params)
            {
                throw new NotImplementedException();
            }

            public Task<ReadOnlyCollection<MessagesSendResult>> SendToUserIdsAsync(MessagesSendParams @params)
            {
                throw new NotImplementedException();
            }

            public Task<bool> RestoreAsync(ulong messageId, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<bool> MarkAsReadAsync(string peerId, long? startMessageId = null, long? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<bool> SetActivityAsync(string userId, MessageActivityType type, long? peerId = null,
                ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<LastActivity> GetLastActivityAsync(long userId)
            {
                throw new NotImplementedException();
            }

            public Task<Chat> GetChatAsync(long chatId, ProfileFields fields = null, NameCase nameCase = null)
            {
                throw new NotImplementedException();
            }

            public Task<ReadOnlyCollection<Chat>> GetChatAsync(IEnumerable<long> chatIds, ProfileFields fields = null,
                NameCase nameCase = null)
            {
                throw new NotImplementedException();
            }

            public Task<ChatPreview> GetChatPreviewAsync(string link, ProfileFields fields)
            {
                throw new NotImplementedException();
            }

            public Task<MessageGetHistoryObject> GetHistoryAsync(MessagesGetHistoryParams @params)
            {
                throw new NotImplementedException();
            }

            public Task<bool> RemoveChatUserAsync(ulong chatId, long? userId = null, long? memberId = null)
            {
                throw new NotImplementedException();
            }

            public Task<LongPollServerResponse> GetLongPollServerAsync(bool needPts = false, uint lpVersion = 2,
                ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<LongPollHistoryResponse> GetLongPollHistoryAsync(MessagesGetLongPollHistoryParams @params)
            {
                throw new NotImplementedException();
            }

            public Task<long> SetChatPhotoAsync(string file)
            {
                throw new NotImplementedException();
            }

            public Task<ReadOnlyCollection<long>> MarkAsImportantAsync(IEnumerable<long> messageIds, bool important = true)
            {
                throw new NotImplementedException();
            }

            public Task<long> SendStickerAsync(MessagesSendStickerParams parameters)
            {
                throw new NotImplementedException();
            }

            public Task<ReadOnlyCollection<HistoryAttachment>> GetHistoryAttachmentsAsync(
                MessagesGetHistoryAttachmentsParams @params)
            {
                throw new NotImplementedException();
            }

            public Task<string> GetInviteLinkAsync(ulong peerId, bool reset)
            {
                throw new NotImplementedException();
            }

            public Task<bool> IsMessagesFromGroupAllowedAsync(ulong groupId, ulong userId)
            {
                throw new NotImplementedException();
            }

            public Task<long> JoinChatByInviteLinkAsync(string link)
            {
                throw new NotImplementedException();
            }

            public Task<bool> MarkAsAnsweredConversationAsync(long peerId, bool? answered = null, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<bool> MarkAsImportantConversationAsync(long peerId, bool? important = null, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<bool> EditAsync(MessageEditParams @params)
            {
                throw new NotImplementedException();
            }

            public Task<ulong> DeleteConversationAsync(long? userId, long? peerId = null, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<ConversationResultObject> GetConversationsByIdAsync(IEnumerable<long> peerIds, IEnumerable<string> fields,
                bool? extended = null, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<GetConversationsResult> GetConversationsAsync(GetConversationsParams getConversationsParams)
            {
                throw new NotImplementedException();
            }

            public Task<GetConversationMembersResult> GetConversationMembersAsync(long peerId, IEnumerable<string> fields,
                ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<GetByConversationMessageIdResult> GetByConversationMessageIdAsync(long peerId,
                IEnumerable<ulong> conversationMessageIds, IEnumerable<string> fields, bool? extended = null,
                ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<SearchConversationsResult> SearchConversationsAsync(string q, IEnumerable<string> fields,
                ulong? count = null, bool? extended = null, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<PinnedMessage> PinAsync(long peerId, ulong? messageId = null)
            {
                throw new NotImplementedException();
            }

            public Task<bool> UnpinAsync(long peerId, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public Task<GetImportantMessagesResult> GetImportantMessagesAsync(
                GetImportantMessagesParams getImportantMessagesParams)
            {
                throw new NotImplementedException();
            }

            public Task<GetRecentCallsResult> GetRecentCallsAsync(IEnumerable<string> fields, ulong? count = null,
                ulong? startMessageId = null, bool? extended = null)
            {
                throw new NotImplementedException();
            }

            public Task<SearchDialogsResponse> SearchDialogsAsync(string query, ProfileFields fields = null, uint? limit = null)
            {
                throw new NotImplementedException();
            }

            public Task<ulong> DeleteDialogAsync(long? userId, long? peerId = null, uint? offset = null, uint? count = null)
            {
                throw new NotImplementedException();
            }

            public Task<bool> MarkAsAnsweredDialogAsync(long peerId, bool answered = true)
            {
                throw new NotImplementedException();
            }

            public Task<bool> MarkAsImportantDialogAsync(long peerId, bool important = true)
            {
                throw new NotImplementedException();
            }

            public Task<MessagesGetObject> GetAsync(MessagesGetParams @params)
            {
                throw new NotImplementedException();
            }

            public Task<ReadOnlyCollection<User>> GetChatUsersAsync(IEnumerable<long> chatIds, UsersFields fields,
                NameCase nameCase)
            {
                throw new NotImplementedException();
            }

            public Task<MessagesGetObject> GetDialogsAsync(MessagesDialogsGetParams @params)
            {
                throw new NotImplementedException();
            }

            public bool AddChatUser(long chatId, long userId)
            {
                throw new NotImplementedException();
            }

            public bool AllowMessagesFromGroup(long groupId, string key)
            {
                throw new NotImplementedException();
            }

            public long CreateChat(IEnumerable<ulong> userIds, string title)
            {
                throw new NotImplementedException();
            }

            public IDictionary<ulong, bool> Delete(IEnumerable<ulong> messageIds, bool? spam = null, ulong? groupId = null,
                bool? deleteForAll = null)
            {
                throw new NotImplementedException();
            }

            public Chat DeleteChatPhoto(out ulong messageId, ulong chatId, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public bool DenyMessagesFromGroup(long groupId)
            {
                throw new NotImplementedException();
            }

            public bool EditChat(long chatId, string title)
            {
                throw new NotImplementedException();
            }

            public VkCollection<Message> GetById(IEnumerable<ulong> messageIds, IEnumerable<string> fields,
                ulong? previewLength = null, bool? extended = null,
                ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public MessageSearchResult Search(MessagesSearchParams @params)
            {
                throw new NotImplementedException();
            }

            public long Send(MessagesSendParams @params)
            {
                _dummy.ActualActions.Add(GroupUpdateType.MessageNew);

                return 0;
            }

            public ReadOnlyCollection<MessagesSendResult> SendToUserIds(MessagesSendParams @params)
            {
                throw new NotImplementedException();
            }

            public bool Restore(ulong messageId, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public bool MarkAsRead(string peerId, long? startMessageId = null, long? groupId = null)
            {
                throw new NotImplementedException();
            }

            public bool SetActivity(string userId, MessageActivityType type, long? peerId = null, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public LastActivity GetLastActivity(long userId)
            {
                throw new NotImplementedException();
            }

            public Chat GetChat(long chatId, ProfileFields fields = null, NameCase nameCase = null)
            {
                throw new NotImplementedException();
            }

            public ReadOnlyCollection<Chat> GetChat(IEnumerable<long> chatIds, ProfileFields fields = null,
                NameCase nameCase = null)
            {
                throw new NotImplementedException();
            }

            public ChatPreview GetChatPreview(string link, ProfileFields fields)
            {
                throw new NotImplementedException();
            }

            public MessageGetHistoryObject GetHistory(MessagesGetHistoryParams @params)
            {
                throw new NotImplementedException();
            }

            public bool RemoveChatUser(ulong chatId, long? userId = null, long? memberId = null)
            {
                throw new NotImplementedException();
            }

            public LongPollServerResponse GetLongPollServer(bool needPts = false, uint lpVersion = 2, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public LongPollHistoryResponse GetLongPollHistory(MessagesGetLongPollHistoryParams @params)
            {
                throw new NotImplementedException();
            }

            public long SetChatPhoto(out long messageId, string file)
            {
                throw new NotImplementedException();
            }

            public ReadOnlyCollection<long> MarkAsImportant(IEnumerable<long> messageIds, bool important = true)
            {
                throw new NotImplementedException();
            }

            public long SendSticker(MessagesSendStickerParams @params)
            {
                throw new NotImplementedException();
            }

            public ReadOnlyCollection<HistoryAttachment> GetHistoryAttachments(MessagesGetHistoryAttachmentsParams @params,
                out string nextFrom)
            {
                throw new NotImplementedException();
            }

            public string GetInviteLink(ulong peerId, bool reset)
            {
                throw new NotImplementedException();
            }

            public bool IsMessagesFromGroupAllowed(ulong groupId, ulong userId)
            {
                throw new NotImplementedException();
            }

            public long JoinChatByInviteLink(string link)
            {
                throw new NotImplementedException();
            }

            public bool MarkAsAnsweredConversation(long peerId, bool? answered = null, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public bool MarkAsImportantConversation(long peerId, bool? important = null, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public bool Edit(MessageEditParams @params)
            {
                throw new NotImplementedException();
            }

            public ulong DeleteConversation(long? userId, long? peerId = null, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public ConversationResultObject GetConversationsById(IEnumerable<long> peerIds, IEnumerable<string> fields,
                bool? extended = null,
                ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public GetConversationsResult GetConversations(GetConversationsParams getConversationsParams)
            {
                throw new NotImplementedException();
            }

            public GetConversationMembersResult GetConversationMembers(long peerId, IEnumerable<string> fields,
                ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public GetByConversationMessageIdResult GetByConversationMessageId(long peerId,
                IEnumerable<ulong> conversationMessageIds, IEnumerable<string> fields,
                bool? extended = null, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public SearchConversationsResult SearchConversations(string q, IEnumerable<string> fields, ulong? count = null,
                bool? extended = null,
                ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public PinnedMessage Pin(long peerId, ulong? messageId = null)
            {
                throw new NotImplementedException();
            }

            public bool Unpin(long peerId, ulong? groupId = null)
            {
                throw new NotImplementedException();
            }

            public GetImportantMessagesResult GetImportantMessages(GetImportantMessagesParams getImportantMessagesParams)
            {
                throw new NotImplementedException();
            }

            public GetRecentCallsResult GetRecentCalls(IEnumerable<string> fields, ulong? count = null,
                ulong? startMessageId = null, bool? extended = null)
            {
                throw new NotImplementedException();
            }

            public ulong DeleteDialog(long? userId, long? peerId = null, uint? offset = null, uint? count = null)
            {
                throw new NotImplementedException();
            }

            public bool MarkAsAnsweredDialog(long peerId, bool answered = true)
            {
                throw new NotImplementedException();
            }

            public bool MarkAsImportantDialog(long peerId, bool important = true)
            {
                throw new NotImplementedException();
            }

            public SearchDialogsResponse SearchDialogs(string query, ProfileFields fields = null, uint? limit = null)
            {
                throw new NotImplementedException();
            }

            public MessagesGetObject Get(MessagesGetParams @params)
            {
                throw new NotImplementedException();
            }

            public ReadOnlyCollection<User> GetChatUsers(IEnumerable<long> chatIds, UsersFields fields, NameCase nameCase)
            {
                throw new NotImplementedException();
            }

            public MessagesGetObject GetDialogs(MessagesDialogsGetParams @params)
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Authorize(IApiAuthParams @params)
        {
            throw new NotImplementedException();
        }

        public void Authorize(ApiAuthParams @params)
        {
            throw new NotImplementedException();
        }

        public void RefreshToken(Func<string> code = null)
        {
            throw new NotImplementedException();
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public bool IsAuthorized { get; }

        public Task AuthorizeAsync(IApiAuthParams @params)
        {
            throw new NotImplementedException();
        }

        public Task RefreshTokenAsync(Func<string> code = null)
        {
            throw new NotImplementedException();
        }

        public Task LogOutAsync()
        {
            throw new NotImplementedException();
        }

        public IUsersCategory Users { get; }
        public IFriendsCategory Friends { get; }
        public IStatusCategory Status { get; }
        public IMessagesCategory Messages { get; private set; }
        public IGroupsCategory Groups { get; }
        public IAudioCategory Audio { get; }
        public IDatabaseCategory Database { get; }
        public IUtilsCategory Utils { get; }
        public IWallCategory Wall { get; }
        public IBoardCategory Board { get; }
        public IFaveCategory Fave { get; }
        public IVideoCategory Video { get; }
        public IAccountCategory Account { get; }
        public IPhotoCategory Photo { get; }
        public IDocsCategory Docs { get; }
        public ILikesCategory Likes { get; }
        public IPagesCategory Pages { get; }
        public IAppsCategory Apps { get; }
        public INewsFeedCategory NewsFeed { get; }
        public IStatsCategory Stats { get; }
        public IGiftsCategory Gifts { get; }
        public IMarketsCategory Markets { get; }
        public IAuthCategory Auth { get; }
        public IExecuteCategory Execute { get; }
        public IPollsCategory PollsCategory { get; }
        public ISearchCategory Search { get; }
        public IStorageCategory Storage { get; }
        public IAdsCategory Ads { get; }
        public INotificationsCategory Notifications { get; }
        public IWidgetsCategory Widgets { get; }
        public ILeadsCategory Leads { get; }
        public IStreamingCategory Streaming { get; }
        public IPlacesCategory Places { get; }
        public INotesCategory Notes { get; set; }
        public IAppWidgetsCategory AppWidgets { get; set; }
        public IOrdersCategory Orders { get; set; }
        public ISecureCategory Secure { get; set; }
        public IStoriesCategory Stories { get; set; }
        public ILeadFormsCategory LeadForms { get; set; }
        public ICaptchaSolver CaptchaSolver { get; }
        public int MaxCaptchaRecognitionCount { get; set; }

        public VkResponse Call(string methodName, VkParameters parameters, bool skipAuthorization = false)
        {
            throw new NotImplementedException();
        }

        public T Call<T>(string methodName, VkParameters parameters, bool skipAuthorization = false,
            params JsonConverter[] jsonConverters)
        {
            throw new NotImplementedException();
        }

        public Task<VkResponse> CallAsync(string methodName, VkParameters parameters, bool skipAuthorization = false)
        {
            throw new NotImplementedException();
        }

        public Task<T> CallAsync<T>(string methodName, VkParameters parameters, bool skipAuthorization = false)
        {
            throw new NotImplementedException();
        }

        public string Invoke(string methodName, IDictionary<string, string> parameters, bool skipAuthorization = false)
        {
            throw new NotImplementedException();
        }

        public Task<string> InvokeAsync(string methodName, IDictionary<string, string> parameters, bool skipAuthorization = false)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset? LastInvokeTime { get; }
        public TimeSpan? LastInvokeTimeSpan { get; }

        public VkResponse CallLongPoll(string server, VkParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<VkResponse> CallLongPollAsync(string server, VkParameters parameters)
        {
            throw new NotImplementedException();
        }

        public string InvokeLongPoll(string server, Dictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }

        public Task<string> InvokeLongPollAsync(string server, Dictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }

        public void SetLanguage(Language language)
        {
            throw new NotImplementedException();
        }

        public Language? GetLanguage()
        {
            throw new NotImplementedException();
        }

        public void Validate(string validateUrl)
        {
            throw new NotImplementedException();
        }

        public int RequestsPerSecond { get; set; }
        public IBrowser Browser { get; set; }
        public IAuthorizationFlow AuthorizationFlow { get; set; }
        public INeedValidationHandler NeedValidationHandler { get; set; }
        public IVkApiVersionManager VkApiVersion { get; set; }
        public string Token { get; }
        public long? UserId { get; set; }
        public event VkApiDelegate OnTokenExpires;


        public VkApiDummy(params GroupUpdateType[] expected)
        {
            ExpectedActions.AddRange(expected);
            Messages = new MessageCategoryDummy(this);
        }

        public void CheckExpectedActions()
        {
            for (int i = 0; i < ExpectedActions.Count; i++)
            {
                if (i >= ActualActions.Count) Assert.True(false, "Ожидаемые и реальные действия не равны");
                Assert.Equal(ExpectedActions[i], ActualActions[i]);
            }
        }
    }
}