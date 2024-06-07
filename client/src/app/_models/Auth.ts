export interface AuthModel {
  email: string;
  password: string;
}

export interface ResetPassword {
  password: string;
  confirmPassword: string;
}

export interface ResetPasswordModel {
  email: string;
  token: string;
  passwordDto: ResetPassword;
}