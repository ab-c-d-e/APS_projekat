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


export interface ReferenceCreateDto { 
    id?: number | null;
    paperId?: number;
    title?: string | null;
    authors?: Array<string> | null;
    journal?: string | null;
    year?: number | null;
    linkedPaperId?: number | null;
}

