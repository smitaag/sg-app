version = 0.1
[default]
[default.deploy]
[default.deploy.parameters]
stack_name = "sg-app"
s3_bucket = "aws-sam-cli-managed-default-samclisourcebucket-tdn9l18u0cy8"
s3_prefix = "sg-app"
region = "us-east-1"
confirm_changeset = true
capabilities = "CAPABILITY_IAM"
disable_rollback = true
image_repositories = []
