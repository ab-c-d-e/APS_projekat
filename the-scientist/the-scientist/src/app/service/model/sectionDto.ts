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
import { SectionType } from './sectionType';


export interface SectionDto { 
    paperId?: number;
    sectionId?: number;
    title?: string | null;
    type?: SectionType;
    subSections?: Array<SectionDto> | null;
}
export namespace SectionDto {
}


