namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    /// <summary> Check if there is already an existing project with the same name as the provided project</summary>
    /// <returns> True if a matching name has been found. </returns>
    public bool CheckProjectExistence(Project project, out string errorMessageKey)
    {
        string projectName = project.Name;
        var alreadyExist =
            (from savedProject in this.Projects
             where savedProject.Name == projectName
             select savedProject)
            .FirstOrDefault();
        if (alreadyExist is not null)
        {
            errorMessageKey = "Model.Project.AlreadyExists";
            return true;
        }
        else
        {
            errorMessageKey = string.Empty;
            return false;
        }
    }

    /// <summary> Gets an existing project with the same name as the provided project name</summary>
    /// <returns> A project object if a matching name has been found, otherwise null. </returns>
    public Project? GetProjectByName(string projectName, out string errorMessageKey)
    {
        if (string.IsNullOrWhiteSpace(projectName))
        {
            errorMessageKey = "Model.Project.InvalidName";
            return null;
        }

        Project? alreadyExist =
            (from savedProject in this.Projects
             where savedProject.Name == projectName
             select savedProject)
            .FirstOrDefault();
        if (alreadyExist is not null)
        {
            errorMessageKey = string.Empty;
            return alreadyExist;
        }
        else
        {
            errorMessageKey = "Model.Project.DoesNotExist";
            return null;
        }
    }

    /// <summary> Adds a NON existing project.</summary>
    /// <returns> True if success . </returns>
    public bool AddNewProject(Project project, out string errorMessageKey)
    {
        if (!project.Validate(out errorMessageKey))
        {
            return false;
        }

        if (this.CheckProjectExistence(project, out errorMessageKey))
        {
            return false;
        }

        project.Created = DateTime.Now;
        project.LastUpdated = DateTime.Now;
        this.Projects.Add(project);
        this.Save();
        this.SaveProjectFile(project);
        return true;
    }

    /// <summary> Saves an existing or NON existing project.</summary>
    /// <returns> True if success. </returns>
    /// <remarks> If a project of same name exists, it will be replaced.</remarks>
    public bool SaveExistingProject(Project project, out string errorMessageKey)
    {
        if (!project.Validate(out errorMessageKey))
        {
            return false;
        }

        Project? maybeProject = this.GetProjectByName(project.Name, out errorMessageKey);
        if (maybeProject is Project existingProject)
        {
            this.Projects.Remove(existingProject);
        }
        else
        {
            return false;
        }

        project.LastUpdated = DateTime.Now;
        this.Projects.Add(project);
        this.Save();
        this.SaveProjectFile(project);
        return true;
    }

    /// <summary> Deletes an existing project with the same name as the provided project name.</summary>
    /// <returns> True is project has been deleted, otherwise false. </returns>
    public bool DeleteProject(string projectName, out string errorMessageKey)
    {
        Project? project = this.GetProjectByName(projectName, out errorMessageKey);
        if (project is not Project existingProject)
        {
            errorMessageKey = "Model.Project.DoesNotExist";
            return false;
        }

        this.Projects.Remove(existingProject);
        this.Save();
        return true;
    }

    public void SaveProjectFile(Project project)
    {
        try
        {
            var projectFileId =
                new FileId(FileManagerModel.Area.User, FileManagerModel.Kind.Json, project.Name);
            this.fileManager.Save(projectFileId, project);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    public Project? LoadProjectFile(string path)
    {
        try
        {
            string serialized = File.ReadAllText(path);
            object? deserialized = this.fileManager.Deserialize<Project>(serialized);
            if (deserialized is Project project)
            {
                return project;
            }

            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return null;
        }
    }
}
