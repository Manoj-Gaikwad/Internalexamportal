using AutoMapper;
using Internalexamportal.Core.Commons.Utils;
using Internalexamportal.Core.Enums;
using Internalexamportal.Core.Features;
using Internalexamportal.Core.Features.AddUser;
using Internalexamportal.Core.Features.Admin;
using Internalexamportal.Core.Features.Admin.testManagement;
using Internalexamportal.Core.Features.AnswerEvaluation;
using Internalexamportal.Core.Features.Candidate;
using Internalexamportal.Core.Features.CandidateLogin;
using Internalexamportal.Core.Features.ClientFeatures;
using Internalexamportal.Core.Features.CreateUser;
using Internalexamportal.Core.Features.ImportCandidates;
using Internalexamportal.Core.Features.Privilege;
using Internalexamportal.Core.Features.Users;
using Internalexamportal.Core.Models;
using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using GetSubjectTopicResult = Internalexamportal.Core.Features.GetSubjectTopicResult;
using QuestionOptionModel = Internalexamportal.Core.Features.Candidate.QuestionOptionModel;

namespace Internalexamportal.Core
{
    public class InternalExamportalCoreMapperProfile : Profile
    {
        public InternalExamportalCoreMapperProfile()
            : this(nameof(Internalexamportal) + nameof(Core) + "Profile")
        {
        }
        protected InternalExamportalCoreMapperProfile(string profileName)
            : base(profileName)
        {
            CreateMap<CreateUserModel, User>();
            CreateMap<CreateCandidateLoginModel, User>();
            CreateMap<SubjectModel, Subject>();
            CreateMap<EditSubjectModel, Subject>();
            CreateMap<GetSubjectModel, Subject>();
            CreateMap<SubjectTopicModule, SubjectTopic>();
            CreateMap<DeleteSubjectResult, Subject>();
            CreateMap<TestInstructionModel, TestInstruction>();
            CreateMap<GetTestSettingResponseModel, Test>();
            CreateMap<TestManagerModel, Test>();
            CreateMap<TestDetails, Test>();
            CreateMap<AddTestQuestionModel, TestQuestion>();
            CreateMap<PublishTestModel, TestPublish>();
            CreateMap<ActivationCodeModel, ActivationCode>();
            CreateMap<UpdateActivationCodeModel, ActivationCode>();
            CreateMap<UpdateTestModel, Test>();
            CreateMap<UpdateTestPublicModel, TestPublish>();

            //User
            CreateMap<User, CandidateModel>()
                    .ForMember(dest => dest.Group, opt => opt.MapFrom(src =>
                              string.Join(" | ", src.CandidateGroup.Select(s => s.Group.Name))));
            CreateMap<User, ExportCandidateModel>()
                    .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src =>
                        (src.RegistrationDate == DateTime.MinValue) ? "NA" : src.RegistrationDate.ToString("dd/MM/yyyy hh:mm tt")))
                    .ForMember(dest => dest.Group, opt => opt.MapFrom(src =>
                              string.Join(" | ", src.CandidateGroup.Select(s => s.Group.Name))));
            CreateMap<User, AdminModel>();
            CreateMap<User, EditCandidateResult>();
            CreateMap<User, GetUserDetailsResult>()
                    .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name));
            CreateMap<User, GetUserResult>();
            CreateMap<AddCandidateModel, User>()
                    .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DOB));
            CreateMap<AddUserModel, User>()
                    .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DOB));
            CreateMap<ImportCandidateModel, User>()
                    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                    .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => Utilities.stringToDate(src.DateOfBirth)));

            //Role
            CreateMap<Role, RoleResult>();

            //Role Permisssion
            CreateMap<RolePermissionModel, RolePermission>().ReverseMap();

            //Permissions
            CreateMap<PermissionResult, Permission>().ReverseMap();

            //Subject
            CreateMap<GetSubjectDetailsModel, Subject>();
            CreateMap<Subject, GetAllSubjectsWithTopicsResult>()
      .ForMember(dest => dest.SubjectTopic, opt => opt.MapFrom(src => src.SubjectTopic));
            CreateMap<SubjectTopic, TopicModel>();
            //Question
            CreateMap<Question, QuestionResultModel>()
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => Utilities.StripHTML(src.Description)))
                    .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject.SubjectName))
                    .ForMember(dest => dest.Topic, opt => opt.MapFrom(src => src.SubjectTopic.Topic))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.QuestionType.Type));
            CreateMap<Question, FilteredQuestionModel>()
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => Utilities.StripHTML(src.Description)))
                    .ForMember(dest => dest.PositiveMark, opt => opt.MapFrom(src => src.QuestionMark.RightMark))
                    .ForMember(dest => dest.NegativeMark, opt => opt.MapFrom(src => src.QuestionMark.NegativeMark));

            //TestQuestion
            CreateMap<TestQuestionRequestModel, TestQuestion>();
            CreateMap<TestQuestion, QuestionsList>()
                    .ForMember(dest => dest.IsMultiSelect, opt => opt.MapFrom(src => (src.Question.QuestionOption.Where(qo => qo.IsAnswer).Count() > 1)));
            CreateMap<Question, QuestionModel>();
            CreateMap<QuestionOption, QuestionOptionModel>();
            CreateMap<TestQuestion, TestQuestionModel>()
                    .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Question.Id))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => Utilities.StripHTML(src.Question.Description)));


            //TestType
            CreateMap<TestType, GetTestTypesResult>();


            // QuestionType
            CreateMap<QuestionType, GetQuestionTypeResult>();

            CreateMap<Test, TestResultModel>();
            //Test
            //CreateMap<Test, GetTestDetailsResult>()
            //    .ForMember(dest => dest.TestDetails, opt => opt.MapFrom(src => src));
            CreateMap<Test, TestDetails>();


            //Test
            CreateMap<Test, GetTestDetailsResult>()
                    .ForMember(dest => dest.TestDetails, opt => opt.MapFrom(src => src));
            CreateMap<TestResultModel, Test>();
            CreateMap<Test, TestModel>()
                    .ForMember(dest => dest.IsResultPublished, opt => opt.MapFrom(src => src.Setting.Any(s => s.TestSettingTypeId == (int)TestSettingEnum.DisplayResult)))
                    .ForMember(dest => dest.TestUserCount, opt => opt.MapFrom(src => src.SubmittedTest.Count()));

            //TestSetting
            CreateMap<TestSettingType, SettingTypeResult>();

            //SubmittedTest
            CreateMap<SubmittedTest, GetExamReportResponseModel>()
                    .ForMember(dest => dest.TotalTime, opt => opt.MapFrom(src => src.Test.Duration.ToString()));
            CreateMap<SubmittedTest, SubmittedTestModel>()
                    .ForMember(dest => dest.TestName, opt => opt.MapFrom(src => src.Test.TestName))
                    .ForMember(dest => dest.TotalQuestion, opt => opt.MapFrom(src => src.Test.TotalQuestion))
                    .ForMember(dest => dest.IsCertificateAllowed, opt => opt.MapFrom(src => src.Percentage >= src.Test.Percentage.GetValueOrDefault()))
                    .ForMember(dest => dest.IsResultPublished, opt => opt.MapFrom(src =>
                                src.Test.Setting.Any(s => s.TestSettingTypeId == (int)TestSettingEnum.DisplayResult)))
                    .ForMember(dest => dest.IsAnswerSheetAllowed, opt => opt.MapFrom(src =>
                                src.Test.Setting.Any(s => s.TestSettingTypeId == (int)TestSettingEnum.DisplayAnswerSheet)));
            CreateMap<SubmittedTest, GetTestResultsResponseModel>()
                    .ForMember(dest => dest.MaximumMark, opt => opt.MapFrom(src => src.Test.TotalMark))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                    .ForMember(dest => dest.TotalMark, opt => opt.MapFrom(src => (src.StatusId == (int)TestStatusEnum.Evaluated) ? src.TotalMark.ToString() : "NA"))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Status));
            CreateMap<SubmittedTest, GetSubmittedAnswersResult>()
                    .ForMember(dest => dest.ObtainedMark, opt => opt.MapFrom(src => src.TotalMark))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                    .ForMember(dest => dest.TestName, opt => opt.MapFrom(src => src.Test.TestName))
                    .ForMember(dest => dest.TotalMark, opt => opt.MapFrom(src => src.MaximumMark));
            CreateMap<SubmittedTest, GetTestResultByIdResponseModel>()
                    .ForMember(dest => dest.MaximumMark, opt => opt.MapFrom(src => src.Test.TotalMark))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                    .ForMember(dest => dest.TestName, opt => opt.MapFrom(src => src.Test.TestName))
                    .ForMember(dest => dest.RollNumber, opt => opt.MapFrom(src => src.User.RollNumber));


            //SubmittedAnswers
            CreateMap<TestQuestion, SubjectiveQuestionModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Question.Id))
                    .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.Question.QuestionTypeId))
                    .ForMember(dest => dest.PositiveMarks, opt => opt.MapFrom(src => src.PositiveMark))
                    .ForMember(dest => dest.NegativeMarks, opt => opt.MapFrom(src => src.NegativeMark))
                    .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question.Description))
                    .ForMember(dest => dest.QuestionOption, opt => opt.MapFrom(src => src.Question.QuestionOption.FirstOrDefault()));
            CreateMap<TestQuestion, ObjectiveQuestionModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Question.Id))
                    .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.Question.QuestionTypeId))
                    .ForMember(dest => dest.PositiveMarks, opt => opt.MapFrom(src => src.PositiveMark))
                    .ForMember(dest => dest.NegativeMarks, opt => opt.MapFrom(src => src.NegativeMark))
                    .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question.Description))
                    .ForMember(dest => dest.QuestionOptions, opt => opt.MapFrom(src => src.Question.QuestionOption));

            //Client
            CreateMap<Client, ClientGridModel>();
            CreateMap<Client, GetClientByIdResult>();
            CreateMap<AddEditClientModel, Client>();

            //Print
            CreateMap<SubmittedTest, ResultPrintModel>()
                    .ForMember(dest => dest.SubmitDate, opt => opt.MapFrom(src => src.SubmitDate.ToString("dd/MM/yyyy")))
                    .ForMember(dest => dest.TotalMark, opt => opt.MapFrom(src => src.Test.TotalMark))
                    .ForMember(dest => dest.ObtainedMark, opt => opt.MapFrom(src => src.TotalMark))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                    .ForMember(dest => dest.TestName, opt => opt.MapFrom(src => src.Test.TestName))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                    .ForMember(dest => dest.CandidateId, opt => opt.MapFrom(src => src.User.RollNumber));

            CreateMap<SubmittedTest, ReportModel>()
                    .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.Percentage.ToString("0.00")))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.FullName))
                    .ForMember(dest => dest.Marks, opt => opt.MapFrom(src => src.ObtainedMarks))
                    .ForMember(dest => dest.RollNumber, opt => opt.MapFrom(src => src.User.RollNumber))
                    .ForMember(dest => dest.Result, opt => opt.MapFrom(src => (src.Percentage >= src.Test.Percentage) ? "PASS" : "FAIL"));

            CreateMap<Test, TestDetailsModel>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TestName))
                    .ForMember(dest => dest.TotalMarks, opt => opt.MapFrom(src => src.TotalMark));


            CreateMap<Subject, GetSubjectResult>();
            CreateMap<DifficultLevel, GetDifficultLevelResult>();
            CreateMap<FilteredQuestionModel, FilterQuestionResult>();
            CreateMap<GetSubjectTopicModel, GetSubjectTopicResult>();
      //      CreateMap<GetTopicModel, GetSubjectTopicResult>();
            CreateMap<GetQuestionModel, GetQuestionResult>();
            //CreateMap<SubjectTopic, GetSubjectTopicResult>();
            CreateMap<SubjectTopic, GetSubjectTopicResult>()
           .ForMember(dest => dest.QuestionsData, opt => opt.MapFrom(src => src.Questions));

            //CreateMap<Question, QuestionDto>();
            CreateMap<QuestionDto, GetSubjectTopicResult>();
           
            CreateMap<Question, QuestionDto>();
            CreateMap<GetTopicModel, GetSubjectTopicResult>();
            CreateMap<GetSubjectDetailsModel, GetSubjectDetailsResult>();
            CreateMap<Subject, GetSubjectDetailsResult>();
            CreateMap<Client, GetClientModel>();
            CreateMap<GetInstructionModel, GetInstructionModelResult>();
            CreateMap<ActivationType, GetTestCodeTypeResult>();
            CreateMap<TestInstruction, GetInstructionModelResult>();
            CreateMap<TestInstruction, InstructionTestResult>();


        }
    }
}
