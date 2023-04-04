import { Component } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ScientificPaperService } from "../service";
import jwt_decode from 'jwt-decode';

@Component({
    selector: 'editable-paper',
    templateUrl: './editable-paper.component.html',
    //styleUrls: ['./editable-paper.component.scss']
})
export class EditablePaperComponent {
    name:string;
    paper:any;
    constructor(private route: ActivatedRoute, private sciPaperService: ScientificPaperService) {
        const jwt=localStorage.getItem('jwt');
        if(jwt) 
        {
            const payload = jwt_decode(jwt);
            this.name=(payload as any).name;
            
            
        }
        else this.name="UnAuthorized";
        this.route.params.subscribe(params => {
            const paperId = params['id'];
            this.sciPaperService.scientificPaperGetFullEditablePaperIdGet(paperId).subscribe((result) =>
                this.paper=result)
        });
    }
}