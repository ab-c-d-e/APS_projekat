/**
 * JWTToken_Auth_API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { UserRoleType } from './userRoleType';


export interface AddUserDto { 
    paperId?: number;
    userName?: string | null;
    role?: UserRoleType;
}
export namespace AddUserDto {
}


