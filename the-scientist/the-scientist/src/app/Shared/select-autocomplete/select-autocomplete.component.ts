import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

interface ItemData {
  item: string;
  selected: boolean;
}

@Component({
  selector: 'multiselect-autocomplete',
  templateUrl: './select-autocomplete.component.html',
  styleUrls: ['./select-autocomplete.component.scss']
})
export class SelectAutocompleteComponent {

  @Output() result = new EventEmitter<{ key: string, data: Array<string> }>();
  newOption?: string;
  @Input() placeholder: string = 'Select Data';
  _data: string[] = [];
  @Input()
  set data(data: string[]) {
    console.log(data);
    this._data = data;
    this._data?.forEach((item: string) => {
      if (this.selectData.find(option => option.item === item))
        this.rawData.push({ item, selected: true });
      else
        this.rawData.push({ item, selected: false });
    });

    this.filteredData = this.selectControl.valueChanges.pipe(
      startWith<string>(''),
      map(value => typeof value === 'string' ? value : this.filterString),
      map(filter => this.filter(filter))
    );
  }
  get data(): string[] {
    return this._data;
  }

  @Input() key: string = '';
  @Input() set preselect(select: string[]) {
    select.forEach(element => {
      this.selectData.push({ item: element, selected: true });
      this.rawData.find(option => option.item === this.newOption);
    });
  }

  selectControl = new FormControl();

  rawData: Array<ItemData> = [];

  selectData: Array<ItemData> = [];

  filteredData: Observable<Array<ItemData>>;
  filterString: string = '';

  constructor() {
    this.filteredData = this.selectControl.valueChanges.pipe(
      startWith<string>(''),
      map(value => typeof value === 'string' ? value : this.filterString),
      map(filter => this.filter(filter))
    );
  }

  filter = (filter: string): Array<ItemData> => {
    this.filterString = filter;
    if (filter.length > 0) {
      return this.rawData.filter(option => {
        return option.item.toLowerCase().indexOf(filter.toLowerCase()) >= 0;
      });
    } else {
      return this.rawData.slice();
    }
  };

  displayFn = (): string => '';

  optionClicked = (event: Event, data: ItemData): void => {
    event.stopPropagation();
    this.toggleSelection(data);
  };

  toggleSelection = (data: ItemData): void => {
    data.selected = !data.selected;

    if (data.selected === true) {
      this.selectData.push(data);
    } else {
      const i = this.selectData.findIndex(value => value.item === data.item);
      this.selectData.splice(i, 1);
    }

    this.selectControl.setValue(this.selectData);
    this.emitAdjustedData();
  };

  emitAdjustedData = (): void => {
    const results: Array<string> = []
    this.selectData.forEach((data: ItemData) => {
      results.push(data.item);
    });
    this.result.emit({ key: this.key, data: results });
  };

  removeChip = (data: ItemData): void => {
    this.toggleSelection(data);
  };

  addOption() {
    if (this.newOption && !this.rawData.find(option => option.item === this.newOption)) {
      const data = { item: this.newOption, selected: true };
      this.rawData.push(data);
      this.selectData.push(data);
      this.selectControl.setValue(this.selectData);
      this.emitAdjustedData();
      this.newOption = '';
    }
    else if (this.rawData.find(option => option.item === this.newOption)) {
      const a = this.rawData.find(option => option.item === this.newOption);
      if (a) {
        this.toggleSelection(a)
      }
    }
  }


}