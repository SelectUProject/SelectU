export interface UpdateUserRolesDTO {
  userId: string;
  removeRoles: string[] | null;
  AddRoles: string[] | null;
}
