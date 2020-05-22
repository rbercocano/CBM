import { TestBed } from '@angular/core/testing';

import { CorpClientService } from './corp-client.service';

describe('CorpoClientService', () => {
  let service: CorpClientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CorpClientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
