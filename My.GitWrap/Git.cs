using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using VitML.Configurator.EdgeServerSource.Exceptions;

namespace My.GitWrap
{
    public class Git
    {

        const string ORIGIN_MASTER = "origin";

        private Repository repo;        
        private UsernamePasswordCredentials user;
        private string email;

        public Git() { }

        public void SetUser(string username, string email, string password)
        {
            if (String.IsNullOrWhiteSpace(username)) throw new ArgumentNullException("username");
            if (String.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("email");
            if (String.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password");

            this.user = new UsernamePasswordCredentials()
            {
                Username = username,
                Password = password,
            };
            this.email = email;            
        }

        public void Connect(string repoPath)
        {
            if (String.IsNullOrWhiteSpace(repoPath)) throw new ArgumentNullException("repoPath");

            RepositoryOptions opt = new RepositoryOptions();            
            try
            {
                repo = new Repository(repoPath, opt);
            }
            catch (RepositoryNotFoundException rnfe)
            {
                throw new GitRepoException("Git repository not found", rnfe);
            }        
        }

        public void Push()
        {
            SaveLocalChanges();
            Fetch();
            int? ahead = repo.Head.TrackingDetails.AheadBy;
            int? behind = repo.Head.TrackingDetails.BehindBy;
            if (behind != null && behind > 0)
                Pull();
            PushOptions opt = new PushOptions();            
            opt.CredentialsProvider = UserCredentialsProvider;
            try
            {
                repo.Network.Push(repo.Head, opt);
            }
            catch (NonFastForwardException nfe)
            {
                throw new GitSyncException("Non fast Forward. Pull first", nfe);
            }
            catch (Exception e)
            {
                throw new GitAccessException("Unauthorized git user", e);
            }
        }
        public void Pull()
        {
            SaveLocalChanges();
            Fetch();
            int? ahead = repo.Head.TrackingDetails.AheadBy;
            int? behind = repo.Head.TrackingDetails.BehindBy;
            if (behind != null && behind > 0)
            {
                Signature comSig = CreateSignature();
                PullOptions opt = new PullOptions();                
                opt.MergeOptions = new MergeOptions();
                opt.MergeOptions.CommitOnSuccess = true;
                opt.MergeOptions.MergeFileFavor = MergeFileFavor.Theirs;
                try
                {
                    MergeResult mergeResult = repo.Network.Pull(comSig, opt);
                    if (mergeResult.Status == MergeStatus.Conflicts)
                    {
                        throw new GitSyncException("Merge conflict. Not auto merged");
                    }
                }
                catch (MergeConflictException mce)
                {
                    throw new GitSyncException("Merge conflict", mce);
                }
            }
        }

        private void SaveLocalChanges()
        {
            RepositoryStatus status = repo.RetrieveStatus();
            foreach (var entry in status.Untracked)
                repo.Index.Add(entry.FilePath);
            foreach (var entry in status.Staged)
                repo.Index.Add(entry.FilePath);
            foreach (var entry in status.Missing)
                repo.Index.Remove(entry.FilePath);
            status = repo.RetrieveStatus();
            foreach (var entry in status.Added)
                repo.Stage(entry.FilePath);
            foreach (var entry in status.Modified)
                repo.Stage(entry.FilePath);
            foreach (var entry in status.Removed)
                repo.Unstage(entry.FilePath);
            status = repo.RetrieveStatus();
            if (status.IsDirty)
                Commit("auto commit;");
        }

        private Signature CreateSignature()
        {
            return new Signature(user.Username, email, DateTimeOffset.Now);
        }

        public void Commit(string message)
        {
            CommitOptions opt = new CommitOptions();
            try
            {
                Commit commit = repo.Commit(message, opt);
            }
            catch (EmptyCommitException ece)
            {
                throw new GitSyncException(ece.Message, ece);
            }
        }

        public void Merge()
        {
            try
            {
                Signature signature = CreateSignature();
                MergeOptions opt = new MergeOptions();
                opt.CommitOnSuccess = true;
                opt.MergeFileFavor = MergeFileFavor.Ours;
                Commit lastCommit = repo.Head.Commits.Last();
                MergeResult res = repo.Merge(lastCommit, signature, opt);
            }
            catch (Exception e)
            {
                throw new GitSyncException("Merge conflict was not resolved", e);
            }
        }
        public void Clone(string gitrepo, string localpath)
        {
            if (String.IsNullOrWhiteSpace(gitrepo)) throw new ArgumentNullException("gitrepo");
            if (String.IsNullOrWhiteSpace(localpath)) throw new ArgumentNullException("localpath");

            var opt = new CloneOptions();
            opt.CredentialsProvider = UserCredentialsProvider;
            try
            {
                string clonedRepoPath = Repository.Clone(gitrepo, localpath, opt);
            }
            catch (NameConflictException nce)
            {
                throw new GitRepoException("Cannot clone to directory as it is not empty", nce);
            }
        }

        private Credentials UserCredentialsProvider(string url, string usernameFromUrl, SupportedCredentialTypes types)
        {
            return user;
        }

        [HandleProcessCorruptedStateExceptions]
        public void Fetch()
        {
            FetchOptions opt = new FetchOptions();
            try
            {
                repo.Fetch(ORIGIN_MASTER, opt);
            }
            catch (Exception ex)
            {
                throw ex;
                //@todo - тут виникає AccessViolationException і repo Dispose                
                //Connect(rootDir); //@todo
            }
        }
    }
}
